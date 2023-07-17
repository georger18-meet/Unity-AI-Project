using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEState : State
{
    public override AllStates ThisState { get => AllStates.AOE; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered AOE State");
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating AOE State");
        CheckAttack(manager);
        if (!FirstCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(PreviousState));
            _attackBase.AttackEnabled = false;
        }
        if (SecondCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(NextState));
        }
    }


    // Not From Base State Class
    // ------------------------------
    private CharacterAI _characterAIRef;
    private AttackBase _attackBase;

    private void CheckAttack(StateManager manager)
    {
        if (!_characterAIRef) _characterAIRef = manager.GetComponent<CharacterAI>();
        if (!_attackBase) _attackBase = manager.transform.GetChild(0).GetComponent<AttackBase>();

        _attackBase.AttackEnabled = _characterAIRef.Attacking;
    }
}
