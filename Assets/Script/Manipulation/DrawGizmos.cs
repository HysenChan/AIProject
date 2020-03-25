using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    public float evadeDistance;
    //领队前方的一个点
    private Vector3 center;
    private Vehicle vehicleScript;
    private float LEADER_BEHIND_DIST;

    private void Start()
    {
        vehicleScript = GetComponent<Vehicle>();
        LEADER_BEHIND_DIST = 2.0f;
    }

    private void Update()
    {
        center = transform.position + vehicleScript.velocity.normalized * LEADER_BEHIND_DIST;
    }

    private void OnDrawGizmos()
    {
        //画出一个位于领队前方的线框球，如果其他角色进入这个范围，就需要激发逃避行为
        Gizmos.DrawWireSphere(center, evadeDistance);
    }
}
