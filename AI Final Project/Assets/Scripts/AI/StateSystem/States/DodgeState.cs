using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : State
{
    public override AllStates ThisState { get => AllStates.Dodge; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Dodge State");
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Dodge State");
        DoDodge(manager);
        if (FirstCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(PreviousState));
            FirstCondition = false;
        }
        if (SecondCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(NextState));
        }
    }

    // Not From Base State Class
    // ------------------------------
    private Transform _charTransform;
    private int _dodgeForce = 1;
    private float _dodgeTime = 1;
    private float _dodgeTimer;
    private int _switchNum = 1;

    private void DoDodge(StateManager manager)
    {
        if (!_charTransform) _charTransform = manager.transform;

        if (_dodgeTimer == 0)
        {
            _switchNum *= -1;
        }

        _dodgeTimer += Time.deltaTime;
        if (_dodgeTimer <= _dodgeTime)
        {
            _charTransform.Translate((_charTransform.right * _switchNum) * _dodgeForce * Time.deltaTime, Space.World);
        }
        else
        {
            _dodgeTimer = 0;
            FirstCondition = true;
        }
    }
}
