using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState : MonoBehaviour
{
    protected StateID stateID;
    public StateID ID
    {
        get { return this.stateID; }
    }
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected FSMSystem fsmSystem;

    public FSMState(FSMSystem fsm)
    {
        this.fsmSystem = fsm;
    }

    /// <summary>
    /// 添加转换条件
    /// </summary>
    /// <param name="transition">转换条件</param>
    /// <param name="stateID">转换的目标状态</param>
    public void AddTransition(Transition transition,StateID stateID)
    {
        if (transition==Transition.NullTransition)
        {
            Debug.LogError(transition + "为空，转换条件不允许为空！");
            return;
        }
        if (stateID==StateID.NullStateID)
        {
            Debug.LogError(stateID + "为空，状态ID不允许为空！");
            return;
        }
        if (map.ContainsKey(transition))
        {
            Debug.LogError(transition + "已经存在，请查看该转换条件是否正确");
            return;
        }
        else
        {
            map.Add(transition, stateID);
        }
    }

    /// <summary>
    /// 删除转换条件
    /// </summary>
    /// <param name="transition">要删除的转换条件</param>
    public void DeleteTransition(Transition transition)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError(transition + "为空，转换条件不允许为空"); return;
        }
        if (map.ContainsKey(transition) == false)
        {
            Debug.LogError(transition + "不存在，请查看该转换条件是否正确");
        }
        else
        {
            map.Remove(transition);
        }
    }

    /// <summary>
    /// 通过转换条件，得到目标状态
    /// </summary>
    /// <param name="transition">转换条件</param>
    /// <returns>返回目标状态</returns>
    public StateID GetTargetStateID(Transition transition)
    {
        if (map.ContainsKey(transition)==false)
        {
            Debug.LogError(transition + "不存在，请查看该转换条件是否正确！");
            return StateID.NullStateID;
        }
        else
        {
            return map[transition];
        }
    }

    /// <summary>
    /// 进入动作
    /// </summary>
    public virtual void DoBeforeEntering() { }
    /// <summary>
    /// 离开动作
    /// </summary>
    public virtual void DoAfterLeaving() { }
    /// <summary>
    /// 输入动作
    /// </summary>
    public abstract void Act();
    /// <summary>
    /// 转移动作
    /// </summary>
    public abstract void Reason();
}
