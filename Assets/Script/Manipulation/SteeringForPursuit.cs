﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForPursuit :Steering
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

        //计算追逐者的前向与逃避者前向之间的夹角
        float relativeDirection = Vector3.Dot(transform.forward, target.transform.forward);

        //如果夹角大于0，且追逐者基本面对着逃避者，那么直接向逃避者当前位置移动
        if ((Vector3.Dot(toTarget, transform.forward) > 0 && relativeDirection < -0.95f))
        {
            //计算预期速度
            desiredVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
            //返回操控向量
            return (desiredVelocity - m_vehicle.velocity);
        }
        //计算预测时间，正比于追逐者与逃避者的距离，反比于追逐者和逃避者的速度和
        float lookaheadTime = toTarget.magnitude / (maxSpeed + target.GetComponent<Vehicle>().velocity.magnitude);
        //计算预期速度
        desiredVelocity = (target.transform.position + target.GetComponent<Vehicle>().velocity * lookaheadTime - transform.position).normalized * maxSpeed;
        //返回操控向量
        return (desiredVelocity - m_vehicle.velocity);
    }
}
