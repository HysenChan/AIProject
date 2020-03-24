using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForSeparation : Steering
{
    //可接受的距离
    public float comfortDistance = 1;
    //当AI角色与邻居之间距离过近时的惩罚因子
    public float multiplierInsideComfortDistance = 2;

    public override Vector3 Force()
    {
        Vector3 steeringForce = new Vector3(0, 0, 0);
        //遍历这个AI角色的邻居列表中的每个邻居
        foreach (GameObject s in GetComponent<Radar>().neighbors)
        {
            //如果s不是当前AI角色
            if ((s!=null)&&(s!=this.gameObject))
            {
                //计算当前AI角色与邻居s之间的距离
                Vector3 toNeighbor = transform.position - s.transform.position;
                float length = toNeighbor.magnitude;
                //计算这个邻居引起的操控力（可以认为是排斥力，大小与距离成反比）
                steeringForce += toNeighbor.normalized / length;
                //如果二者之间距离大于可接受距离，排斥力再乘以一个额外因子
                if (length<comfortDistance)
                {
                    steeringForce *= multiplierInsideComfortDistance;
                }
            }
        }
        return steeringForce;
    }
}
