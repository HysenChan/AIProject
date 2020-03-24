using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForCohesion : Steering
{
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;

    private void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
    }

    public override Vector3 Force()
    {
        //操控向量
        Vector3 steeringForce = new Vector3(0, 0, 0);
        //AI角色的所有邻居的质心，即平均位置
        Vector3 centerOfMass = new Vector3(0, 0, 0);
        //AI角色的邻居的数量
        int neighborCount = 0;
        //遍历邻居列表中的每个邻居
        foreach (GameObject s in GetComponent<Radar>().neighbors)
        {
            //如果s不是当前AI角色
            if ((s!=null)&&(s!=this.gameObject))
            {
                //累加s的位置
                centerOfMass += s.transform.position;
                //邻居数量加1
                neighborCount++;
            }
        }
        //如果邻居数量大于0
        if (neighborCount>0)
        {
            //将位置的累加值除以邻居数量，得到平均值
            centerOfMass /= (float)neighborCount;
            //预期速度为邻居位置平均值与当前位置之差
            desiredVelocity = (centerOfMass - transform.position).normalized * maxSpeed;
            //操控向量=预期速度-当前速度
            steeringForce = desiredVelocity - m_vehicle.velocity;
        }
        return steeringForce;
    }
}
