using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public bool TargetInAttackRange;


    public override AllStates ThisState { get => AllStates.Attack; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Attack State");
        manager.DisplayState = AllStates.Attack;
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Attack State");
        if (!TargetInAttackRange)
        {
            manager.SwitchState(manager._chaseState);
        }
    }
}
