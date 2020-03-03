using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMFramework
{
    public class DeadState : FSMState
    {
        public DeadState()
        {
            //设置当前状态
            stateID = FSMStateID.Dead;
        }

        public override void Reason(Transform player, Transform npc)
        {

        }

        public override void Act(Transform player, Transform npc)
        {
            //TODO:播放死亡动画
            //Animation animComponent = npc.GetComponent<Animation>();
            //animComponent.CrossFade("Death");
        }
    }
}
