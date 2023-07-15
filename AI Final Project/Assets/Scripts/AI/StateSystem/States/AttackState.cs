using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public override AllStates ThisState { get => AllStates.Attack; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Attack State");
        manager.DisplayState = AllStates.Attack;
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Attack State");
        if (!FirstCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(PreviousState));
        }
    }
}
