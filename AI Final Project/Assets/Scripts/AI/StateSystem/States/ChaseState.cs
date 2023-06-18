using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public bool TargetInRange;
    public bool TargetInAttackRange;


    public override AllStates ThisState { get => AllStates.Chase; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Chase State");
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Chase State");
        if (!TargetInRange)
        {
            manager.SwitchState(manager._idleState);
        }
        if (TargetInAttackRange)
        {
            manager.SwitchState(manager._attackState);
        }
    }
}
