using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AILocomotion是Vehicle的派生类
/// </summary>
public class AILocomotion : Vehicle
{
    //AI角色的角色控制器
    private CharacterController controller;

    //AI角色的RigidBody
    private Rigidbody theRigidbody;

    //AI角色每次移动的距离
    private Vector3 moveDistance;

    // Start is called before the first frame update
    new void Start()
    {
        //获得角色控制器（如果有的话）
        controller = GetComponent<CharacterController>();
        //获得AI角色的Rigidbody（如果有的话）
        theRigidbody = GetComponent<Rigidbody>();
        moveDistance = new Vector3(0, 0, 0);

        //调用基类的Start()函数，进行所需的初始化
        base.Start();
    }

    //物理相关操作在FixedUpdate()中更新
    void FixedUpdate()
    {
        //计算速度；Vt=V0+at
        velocity += acceleration * Time.fixedDeltaTime;
        //限制速度，要低于最大速度
        if (velocity.sqrMagnitude > sqrMaxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
        //计算AI角色的移动距离   S=Vt*t
        moveDistance = velocity * Time.fixedDeltaTime;
        //如果要求AI角色在平面上移动，那么将y置为0
        if (isPlanar)
        {
            velocity.y = 0;
            moveDistance.y = 0;
        }

        //如果已经为AI角色添加了角色控制器，那么利用角色控制器时其移动
        if (controller != null)
        {
            controller.SimpleMove(velocity);
        }
        //如果AI角色没有角色控制器，也没有Rigidbody
        //或AI角色拥有Rigidbody，但是要由动力学的方式控制它的运动
        else if (theRigidbody == null || theRigidbody.isKinematic)
        {
            transform.position += moveDistance;
        }
        //用Rigidbody控制AI角色的移动
        else
        {
            theRigidbody.MovePosition(theRigidbody.position + moveDistance);
        }
        //更新朝向，如果速度大于一个阈值（为了防止抖动）
        if (velocity.sqrMagnitude > 0.00001)
        {
            //通过当前朝向与速度方向的插值，计算新的朝向
            Vector3 newForward = Vector3.Slerp(transform.forward, velocity, damping * Time.deltaTime);
            //将y设置为0
            if (isPlanar)
            {
                newForward.y = 0;
            }
            //将向前的方向设置为新的朝向
            transform.forward = newForward;
        }
        //播放行走动画
        //gameObject.GetComponent<Animation>().Play("Walk");
    }
}
