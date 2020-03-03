using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMFramework
{
    public class AttackState : FSMState
    {
        public AttackState(Transform[] wp)
        {
            //这个构造器首先接受传来的巡逻点transform数组，将它们存储在局部变量中
            waypoints = wp;
            //设置状态编号
            stateID = FSMStateID.Attacking;
            //设置转向速度与移动速度
            curRotSpeed = 12.0f;
            curSpeed = 100.0f;

            //从巡逻点数组中随机选择一个，作为当前目标点
            FindNextPoint();
        }

        public override void Reason(Transform player, Transform npc)
        {
            //计算与玩家的距离
            float dist = Vector3.Distance(npc.position, player.position);

            //如果与玩家的距离大于攻击距离而小于追逐距离，那么转到追逐状态
            if (dist >= attackDistance && dist < chaseDistance)
            {
                Debug.Log("Switch to Chase State");
                npc.GetComponent<AIController>().SetTransition(Transition.SawPlayer);
            }
            //如果与玩家距离超出追逐距离，那么回到巡逻状态
            else if (dist >= chaseDistance)
            {
                Debug.Log("Switch to Patrol State");
                npc.GetComponent<AIController>().SetTransition(Transition.LostPlayer);
            }
        }

        public override void Act(Transform player, Transform npc)
        {
            //将玩家位置设置为目标点
            destPos = player.position;

            //转向目标
            Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
            npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            //TODO:发射子弹，播放射击动画
            //Animation animComponent = npc.GetComponent<Animation>();
            //npc.GetComponent<AIController>().ShootBullet();
            //animComponent.CrossFade("StandingFire");
        }
    }
}
