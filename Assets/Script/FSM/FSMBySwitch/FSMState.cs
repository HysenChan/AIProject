using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState
{
    protected StateID stateID;
    public StateID ID
    {
        get { return this.stateID; }
    }
    protected Dictionary<Transition, StateID> dicTrans = new Dictionary<Transition, StateID>();
    protected FSMSystem fsmSytem;

    public FSMState(FSMSystem fsmSytem)
    {
        this.fsmSytem = fsmSytem;
    }

    /// <summary>
    /// 添加转换条件
    /// </summary>
    /// <param name="trans">转换条件</param>
    /// <param name="stateID">转换的目标状态</param>
    public void AddTransition(Transition trans, StateID stateID)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError(trans + "为空，转换条件不允许为空"); return;
        }
        if (stateID == StateID.NullStateID)
        {
            Debug.LogError(stateID + "为空，状态ID不允许为空"); return;
        }
        if (dicTrans.ContainsKey(trans))
        {
            Debug.LogError(trans + "已经存在，请查看该转换条件是否正确");
        }
        else
        {
            dicTrans.Add(trans, stateID);
        }
    }

    /// <summary>
    /// 删除转换条件
    /// </summary>
    /// <param name="trans">需删除的转换条件</param>
    public void DeleteTransition(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError(trans + "为空，转换条件不允许为空"); return;
        }
        if (dicTrans.ContainsKey(trans) == false)
        {
            Debug.LogError(trans + "不存在，请查看该转换条件是否正确");
        }
        else
        {
            dicTrans.Remove(trans);
        }
    }

    /// <summary>
    /// 通过转换条件，得到目标状态
    /// </summary>
    /// <param name="trans">转换条件</param>
    /// <returns>返回目标状态</returns>
    public StateID GetTargetStateID(Transition trans)
    {
        if (dicTrans.ContainsKey(trans) == false)
        {
            Debug.LogError(trans + "不存在，请查看该转换条件是否正确");
            return StateID.NullStateID;
        }
        else
        {
            return dicTrans[trans];
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
