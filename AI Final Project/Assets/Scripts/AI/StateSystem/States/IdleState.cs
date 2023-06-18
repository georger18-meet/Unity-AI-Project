using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Idle State");
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Idle State");
    }
}
