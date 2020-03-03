using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMFramework
{
    public class ChaseState : FSMState
    {
        public ChaseState(Transform[] wp)
        {
            //这个构造器首先接受传来的巡逻点transform数组，将他们存储在局部变量中
            waypoints = wp;
            //设置状态编号
            stateID = FSMStateID.Chasing;

            //设置转向速度与移动速度
            curRotSpeed = 6.0f;
            curSpeed = 160.0f;

            //从巡逻点数组中随机选择一个，作为当前目标点
            FindNextPoint();
        }
        
        public override void Reason(Transform player, Transform npc)
        {
            //将玩家位置设置为目标点
            destPos = player.position;
            //检查与玩家的距离
            //如果小于攻击距离，那么转换到攻击状态
            float dist = Vector3.Distance(npc.position, destPos);
            if (dist <= attackDistance)
            {
                Debug.Log("Switch to Attack state");
                npc.GetComponent<AIController>().SetTransition(Transition.ReachPlayer);
            }
            //如果与玩家距离超出追逐距离，那么回到巡逻状态
            else if (dist >= chaseDistance)
            {
                Debug.Log("Switch to Patrol state");
                npc.GetComponent<AIController>().SetTransition(Transition.LostPlayer);
            }
        }

        public override void Act(Transform player, Transform npc)
        {
            //将玩家位置设为目标点
            destPos = player.position;

            //转向目标点
            Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
            npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            //向前移动
            CharacterController controller = npc.GetComponent<CharacterController>();
            controller.SimpleMove(npc.transform.forward * Time.deltaTime * curSpeed);

            //TODO:播放奔跑动画
            //Animation animComponent = npc.GetComponent<Animation>();
            //animComponent.CrossFade("Run");
        }
    }
}
