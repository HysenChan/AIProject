using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringFollowPath : Steering
{
    //由节点数组表示的路径
    public GameObject[] waypoints = new GameObject[4];
    //目标点
    private Transform target;
    //当前的路点
    private int currentNode;
    //与路点的距离小于这个值，认为已经到达，可以向下一个路点触发
    public float arriveDistance;
    private float sqrArriveDistance;

    //路点的数量
    private int numberOfNodes;
    //操控力
    private Vector3 force;
    //预期速度
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private bool isPlanar;
    //当与目标小于这个距离时，开始减速
    public float slowDownDistance;

    private void Start()
    {
        //存储路点数组中的路点个数
        numberOfNodes = waypoints.Length;
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        isPlanar = m_vehicle.isPlanar;
        //设置当前路点为第0个路点
        currentNode = 0;
        //设置当前路点为目标点
        target = waypoints[currentNode].transform;
        arriveDistance = 1.0f;
        sqrArriveDistance = arriveDistance * arriveDistance;
    }

    public override Vector3 Force()
    {
        force = new Vector3(0, 0, 0);
        Vector3 dist = target.position - transform.position;
        if (isPlanar)
        {
            dist.y = 0;
        }
        //如果当前路点已经是路点数组中的最后一个
        if (currentNode==numberOfNodes-1)
        {
            //如果与当前路点的距离大于减速距离
            if (dist.magnitude>slowDownDistance)
            {
                //求出预期速度
                desiredVelocity = dist.normalized * maxSpeed;
                //计算操控向量
                force = desiredVelocity - m_vehicle.velocity;
            }
            else
            {
                //与当前路点距离小于减速距离，开始减速，计算操控向量
                desiredVelocity = dist - m_vehicle.velocity;
                force = desiredVelocity - m_vehicle.velocity;
            }
        }
        else
        {
            //当前路点不是路点数组中的最后一个，即正走向中间路点
            if (dist.sqrMagnitude<sqrArriveDistance)
            {
                //如果与当前路点距离的平方小于到达距离的平方
                //可以开始靠近下一个路点，将下一个路点设置为目标点
                currentNode++;
                target = waypoints[currentNode].transform;
            }
            //计算预期速度和操控向量
            desiredVelocity = dist.normalized * maxSpeed;
            force = desiredVelocity - m_vehicle.velocity;
        }
        return force;
    }
}
