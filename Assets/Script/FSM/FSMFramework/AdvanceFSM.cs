using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMFramework
{
    /// <summary>
    /// 枚举，为可能的转换分配编号；
    /// </summary>
    public enum Transition
    {
        SawPlayer = 0,    //看到玩家
        ReachPlayer,     //接近玩家，即玩家在攻击范围内
        LostPlayer,       //玩家离开视线
        NoHealth,        //死亡
    }

    /// <summary>
    /// 枚举，为可能的转换分配编号ID；
    /// </summary>
    public enum FSMStateID
    {
        Patrolling = 0,       //巡逻的状态编号为0
        Chasing,            //追逐的状态编号为1
        Attacking,          //攻击的状态编号为2
        Dead,               //死亡的状态编号为3
    }

    public class AdvanceFSM :FSM
    {
        //FSM中的所有状态（多个FSMState）组成的列表
        private List<FSMState> fsmStates;

        //当前状态的编号
        private FSMStateID currentStateID;
        public FSMStateID CurrentStateID { get { return this.currentStateID; } }

        //当前状态
        private FSMState currentState;
        public FSMState CurrentState { get { return this.currentState; } }

        public AdvanceFSM()
        {
            //新建一个空的状态列表
            fsmStates = new List<FSMState>();
        }

        //向状态列表中加入一个新的状态
        public void AddFSMState(FSMState fsmState)
        {
            //检查要加入的新状态是否为空，如果是空，报告错误
            if (fsmState==null)
            {
                Debug.LogError("FSM ERROR:Null reference is not allowed");
            }

            //如果插入这个状态时，列表还是空的，那么将它加入列表并返回
            if (fsmStates.Count==0)
            {
                fsmStates.Add(fsmState);
                currentState = fsmState;
                currentStateID = fsmState.ID;
                return;
            }

            //检查要加入的状态是否已经在列表中，如果是，报告错误并返回
            foreach (FSMState state in fsmStates)
            {
                if (state.ID == fsmState.ID)
                {
                    Debug.LogError("FSM ERROR:Trying to add a state that was already inside the list");
                    return;
                }
            }
            //如果要加入的状态不在列表中，那么将它加入列表
            fsmStates.Add(fsmState);
        }

        //从状态列表中删除一个状态
        public void DeleteState(FSMStateID fsmStateID)
        {
            //搜索整个状态列表，如果要删除的状态在列表中，那么将它移除，否则报错
            foreach (FSMState state in fsmStates)
            {
                if (state.ID== fsmStateID)
                {
                    fsmStates.Remove(state);
                    return;
                }
            }
            Debug.LogError("FSM ERROR:The state passed was not on the list.Impossible to delete it!");
        }

        //根据当前状态，和参数中传递的转换，转移到新状态
        public void PerformTransition(Transition trans)
        {
            //根据当前的状态类，以trans为参数调用它的GetOutputState方法，
            //确定转移后新状态的编号
            FSMStateID id = currentState.GetOutputState(trans);

            //将当前状态编号设置为刚刚返回的新状态编号
            currentStateID = id;
            //根据状态编号查找状态列表，这个查找是通过一个遍历过程实现的
            //将当前状态设置为查找到的状态
            foreach (FSMState state in fsmStates)
            {
                if (state.ID==currentStateID)
                {
                    currentState = state;
                    break;
                }
            }
        }
    }
}