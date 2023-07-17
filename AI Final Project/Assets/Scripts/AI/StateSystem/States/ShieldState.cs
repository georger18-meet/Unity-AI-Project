using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldState : State
{
    public override AllStates ThisState { get => AllStates.Shield; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Shield State");
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Shield State");
        CheckShield(manager);
        if (FirstCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(PreviousState));
        }
        //if (SecondCondition)
        //{
        //    manager.SwitchState(manager.GetStatesHolder.GetStateInDict(NextState));
        //}
    }


    // Not From Base State Class
    // ------------------------------
    private JediAI _characterAIRef;
    private GameObject _shieldObj;

    private void CheckShield(StateManager manager)
    {
        if (!_characterAIRef) _characterAIRef = manager.GetComponent<JediAI>();
        if (!_shieldObj) _shieldObj = _characterAIRef.ShieldObj;

        _shieldObj.SetActive(SecondCondition);
    }
}
