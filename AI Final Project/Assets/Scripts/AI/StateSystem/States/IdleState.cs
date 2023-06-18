using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public bool TargetInRange;


    public override AllStates ThisState { get => AllStates.Idle; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Idle State");
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Idle State");
        if (TargetInRange)
        {
            manager.SwitchState(manager._chaseState);
        }
    }
}
