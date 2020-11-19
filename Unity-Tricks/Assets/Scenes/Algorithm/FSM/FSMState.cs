using UnityEngine;
using System.Collections.Generic;

// 状态转换条件
public enum Transition
{
    NullTransition = 0,
    FindPlayer = 1,
    LosePlayer = 2,
}

// 状态唯一标识
public enum StateId
{
    NullStateId = 0,
    Patrol = 1,
    Chase = 2,
}

// State基类
public abstract class FSMState
{
    private Dictionary<Transition, StateId> transDict = new Dictionary<Transition, StateId>();
    public StateId stateId;
    public FSMSystem fsm;

    public void AddTransition(Transition trans, StateId id)
    {
        if (trans == Transition.NullTransition || id == StateId.NullStateId)
        {
            Debug.LogError("Transition or StateId is Non-Existence");
            return;
        }
        if (!transDict.ContainsKey(trans))
        {
            transDict.Add(trans, id);
        }
        else
        {
            Debug.LogError("Transition is Existence");
        }
    }

    public void RemoveTransition(Transition trans)
    {
        if (transDict.ContainsKey(trans))
        {
            transDict.Remove(trans);
        }
        else
        {
            Debug.LogWarning("Transition is Non-Existence");
        }
    }

    public StateId GetState(Transition trans)
    {
        if (transDict.ContainsKey(trans))
        {
            return transDict[trans];
        }
        else
        {
            return StateId.NullStateId;
        }
    }

    // 进入State之前
    public virtual void DoBeforeEnter() { }

    // 退出State之前
    public virtual void DoBeforeExit() { }

    // 在State中
    public abstract void DoUpdate();
}
