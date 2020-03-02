using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForEvade :Steering
{
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
    }

    public override Vector3 Force()
    {
        Vector3 toTarget = target.transform.position - transform.position;
        //向前预测的时间
        float lookaheadTime = toTarget.magnitude / (maxSpeed + target.GetComponent<Vehicle>().velocity.magnitude);
        //计算预测速度
        desiredVelocity = (transform.position - (target.transform.position + target.GetComponent<Vehicle>().velocity * lookaheadTime)).normalized * maxSpeed;
        //返回操控向量
        return (desiredVelocity - m_vehicle.velocity);
    }
}
