using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForAlignment : Steering
{
    public override Vector3 Force()
    {
        //当前AI角色的邻居的平均朝向
        Vector3 averageDirection = new Vector3(0, 0, 0);
        //邻居的数量
        int neighborCount = 0;
        //遍历当前AI角色的所有邻居
        foreach (GameObject s in GetComponent<Radar>().neighbors)
        {
            //如果s不是当前AI角色
            if ((s != null) && (s != this.gameObject))
            {
                //将s的朝向向量加到averageDirection之中
                averageDirection += s.transform.forward;
                //邻居数量加1
                neighborCount++;
            }
        }
        //如果邻居数量大于0
        if (neighborCount>0)
        {
            //将累加得到的朝向向量除以邻居的个数，求出平均朝向向量
            averageDirection /= (float)neighborCount;
            //平均朝向向量减去当前朝向向量，得到操控向量
            averageDirection -= transform.forward;
        }
        return averageDirection;
    }
}
