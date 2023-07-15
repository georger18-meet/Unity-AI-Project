using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public override AllStates ThisState { get => AllStates.Attack; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Attack State");
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Attack State");
        if (!FirstCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(PreviousState));
        }
        CheckAttack(manager);
    }


    // Not From Base State Class
    // ------------------------------
    private CharacterAI _characterAIRef;
    private AttackBase _attackBase;

    private void CheckAttack(StateManager manager)
    {
        if (!_characterAIRef) _characterAIRef = manager.GetComponent<CharacterAI>();
        if (!_attackBase) _attackBase = manager.GetComponent<AttackBase>();

        _attackBase.AttackEnabled = _characterAIRef.Attacking;
    }
}
