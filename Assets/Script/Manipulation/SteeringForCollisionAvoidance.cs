using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForCollisionAvoidance : Steering
{
    public bool isPlanar;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private float maxForce;
    //避免障碍所产生的操控力
    public float avoidanceForce;
    //能向前看到的最大距离
    public float MAX_SEE_AHEAD = 2.0f;
    //场景中的所有碰撞体组成的数组
    private GameObject[] allColliders;

    private void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        maxForce = m_vehicle.maxForce;
        isPlanar = m_vehicle.isPlanar;

        //如果避免障碍所产生的操控力大于最大操控力，将它截断到最大操控力
        if (avoidanceForce>maxForce)
        {
            avoidanceForce = maxForce;
        }
        //存储场景中的所有碰撞体，即Tag为Obstacle的那些游戏体
        allColliders = GameObject.FindGameObjectsWithTag("Obstacle");
    }

    public override Vector3 Force()
    {
        RaycastHit hit;
        Vector3 force = new Vector3(0, 0, 0);
        Vector3 velocity = m_vehicle.velocity;
        Vector3 normalizedVelocity = velocity.normalized;
        //画一条射线，需要考查与这条射线相交的碰撞体
        Debug.DrawLine(transform.position, transform.position + normalizedVelocity * MAX_SEE_AHEAD * (velocity.magnitude / maxSpeed));
        if (Physics.Raycast(transform.position,normalizedVelocity,out hit,MAX_SEE_AHEAD*velocity.magnitude/maxSpeed))
        {
            //如果射线与某个碰撞体相交，表示可能与改碰撞体发生碰撞
            Vector3 ahead = transform.position + normalizedVelocity * MAX_SEE_AHEAD*(velocity.magnitude / maxSpeed);
            //计算避免碰撞所需的操控力
            force = ahead - hit.collider.transform.position;
            force *= avoidanceForce;
            if (isPlanar)
            {
                force.y = 0;
            }
            //将这个碰撞体的颜色变为绿色，其他都变为灰色
            foreach (GameObject c in allColliders)
            {
                if (hit.collider.gameObject==c)
                {
                    Material material = c.GetComponent<Renderer>().material;
                    material.color = Color.black;
                }
                else
                {
                    Material material = c.GetComponent<Renderer>().material;
                    material.color = Color.white;
                }
            }
        }
        else
        {
            //如果向前看的有限范围内，没有发生碰撞的可能
            //将所有碰撞体设为灰色
            foreach (GameObject c in allColliders)
            {
                Material material = c.GetComponent<Renderer>().material;
                material.color = Color.white;
            }
        }
        //返回操控力
        return force;
    }
}
