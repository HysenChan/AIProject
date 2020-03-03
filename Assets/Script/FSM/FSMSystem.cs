﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
    private Dictionary<StateID, FSMState> fsmState = new Dictionary<StateID, FSMState>();
    private FSMState currentState;

    /// <summary>
    /// 更新当前状态行为
    /// </summary>
    public void UpdateFSM()
    {
        currentState.Act();
        currentState.Reason();
    }

    /// <summary>
    /// 添加状态
    /// </summary>
    /// <param name="state">需要管理的状态</param>
    public void AddState(FSMState state)
    {
        if (state==null)
        {
            Debug.LogError(state + "为空");
            return;
        }
        if (currentState==null)
        {
            currentState = state;
        }
        if (fsmState.ContainsValue(state))
        {
            Debug.LogError(state + "已经存在");
        }
        else
        {
            fsmState.Add(state.ID, state);
        }
    }

    /// <summary>
    /// 删除状态
    /// </summary>
    /// <param name="id">需要删除状态的ID</param>
    /// <returns>删除成功返回true，否则返回false</returns>
    public bool DeleteState(StateID id)
    {
        if (id==StateID.NullStateID)
        {
            Debug.LogError(id + "为空");
            return false;
        }
        if (fsmState.ContainsKey(id)==false)
        {
            Debug.LogError(id + "不存在");
            return false;
        }
        else
        {
            fsmState.Remove(id);
            return true;
        }
    }

    /// <summary>
    /// 执行转换
    /// </summary>
    /// <param name="transition">转换条件</param>
    public void PerformTransition(Transition transition)
    {
        if (transition==Transition.NullTransition)
        {
            Debug.LogError(transition + "为空");
            return;
        }
        StateID targetID = currentState.GetTargetStateID(transition);
        if (fsmState.ContainsKey(targetID)==false)
        {
            Debug.LogError(targetID + "不存在");
            return;
        }
        FSMState targetState = fsmState[targetID];
        currentState.DoAfterLeaving();
        targetState.DoBeforeEntering();
        currentState = targetState;
    }
}
