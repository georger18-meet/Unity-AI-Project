using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public override AllStates ThisState { get => AllStates.Chase; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Chase State");
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Chase State");
        if (!FirstCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(PreviousState));
        }
        if (SecondCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(NextState));
        }
    }
}
