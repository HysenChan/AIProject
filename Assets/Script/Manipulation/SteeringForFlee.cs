using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForFlee : Steering
{
    //需要寻找的目标物体
    public GameObject target;

    //设置使AI角色意识到危险并开始逃跑的范围
    public float fearDistance = 20;

    //逃跑预期速度
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
        Vector3 tmpPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 tmpTargetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        //如果AI角色与目标的距离大于逃跑距离，那么返回0向量
        if (Vector3.Distance(tmpPos,tmpTargetPos)>fearDistance)
        {
            return new Vector3(0, 0, 0);
        }
        //如果AI角色与目标的距离小于逃跑距离，那么计算逃跑所需的操控向量
        desiredVelocity=(transform.position-target.transform.position).normalized*maxSpeed;
        return (desiredVelocity - m_vehicle.velocity);
    }
}
