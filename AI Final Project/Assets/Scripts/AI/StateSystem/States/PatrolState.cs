using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public override AllStates ThisState { get => AllStates.Patrol; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Patrol State");
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Patrol State");
        if (FirstCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(NextState));
        }
        else if (SecondCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(PreviousState));
        }
    }
}
