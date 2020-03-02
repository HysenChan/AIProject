using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForArrive : Steering
{
    //是否仅在二维平面上运动
    private bool isPlanar;

    //当与目标小于这个距离时，开始减速
    public float slowDownDistance;

    //需要寻找的目标物体
    public GameObject target;

    //追逐预期速度
    private Vector3 desiredVelocity;

    //获得被操控AI角色，以便查询这个AI角色的最大速度等信息
    private Vehicle m_vehicle;

    //最大速度
    private float maxSpeed;


    private void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        isPlanar = m_vehicle.isPlanar;
    }

    public override Vector3 Force()
    {
        //计算AI角色与目标之间的距离
        Vector3 toTarget = target.transform.position - transform.position;

        //预期速度
        Vector3 desiredVelocity;

        //返回的操控向量
        Vector3 returnForce;

        if (isPlanar)
        {
            toTarget.y = 0;
        }

        float distance = toTarget.magnitude;
        //如果与目标之间的距离大于所设置的减速半径
        if (distance>slowDownDistance)
        {
            //预期速度是AI角色与目标点之间的距离
            desiredVelocity = toTarget.normalized * maxSpeed;
            //返回预期速度与当前速度的差
            returnForce = desiredVelocity - m_vehicle.velocity;
        }
        else
        {
            //计算预期速度，并返回预期速度与当前速度的差
            desiredVelocity = toTarget - m_vehicle.velocity;
            //返回预期速度与当前速度的差
            returnForce = desiredVelocity - m_vehicle.velocity;
        }
        return returnForce;
    }

    private void OnDrawGizmos()
    {
        //在目标周围画绿线，显示出减速范围
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.transform.position, slowDownDistance);
    }
}
