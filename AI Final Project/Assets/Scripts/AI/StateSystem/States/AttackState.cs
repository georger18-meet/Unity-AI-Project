using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Attack State");
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Attack State");
    }
}
