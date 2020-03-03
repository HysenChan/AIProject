using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMFramework
{
    public class PatrolState : FSMState
    {
        public PatrolState(Transform[] wp)
        {
            //这个构造器首先接受传来的巡逻点transform数组，将它们存储在局部变量中
            waypoints = wp;
            //设置状态编号
            stateID = FSMStateID.Patrolling;
            //设置转向速度与移动速度
            curRotSpeed = 6.0f;
            curSpeed = 80.0f;
        }

        //这个方法决定是否需要转换状态，以及发生哪种转换
        public override void Reason(Transform player, Transform npc)
        {
            //检查AI角色与玩家的距离，如果小于追逐距离
            if (Vector3.Distance(npc.position, player.position) <= chaseDistance)
            {
                Debug.Log("Switch to Chase State");
                //设置转换为“看到玩家”
                npc.GetComponent<AIController>().SetTransition(Transition.SawPlayer);
            }
        }

        //这个方法定义了在这个状态下AI角色的行为
        public override void Act(Transform player, Transform npc)
        {
            //如果已经到达当前巡逻点，那么调用FindNextPoint函数，选择下一个巡逻点
            if (Vector3.Distance(npc.position, destPos) <= arriveDistance)
            {
                Debug.Log("Reached to the destination point\ncalculating the next point");
                FindNextPoint();
            }

            //转向
            Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
            npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            //获得角色控制器组件，控制AI角色向前移动
            CharacterController controller = npc.GetComponent<CharacterController>();
            controller.SimpleMove(npc.transform.forward * Time.deltaTime * curSpeed);

            //TODO:播放行走动画
            //Animation animComponent = npc.GetComponent<Animation>();
            //animComponent.CrossFade("Walk");
        }
    }
}
