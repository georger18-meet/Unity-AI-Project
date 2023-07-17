using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealState : State
{
    public override AllStates ThisState { get => AllStates.Heal; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Heal State");
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Heal State");
        if (FirstCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(PreviousState));
        }
        if (SecondCondition)
        {
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(NextState));
        }
        DoHeal(manager);
    }

    // Not From Base State Class
    // ------------------------------
    private HealthHandler _healthHandler;
    private int _healAmount = 1;
    private float _healSpeed = 2;
    private float _healTimer;

    private void DoHeal(StateManager manager)
    {
        if (!_healthHandler) _healthHandler = manager.GetComponent<HealthHandler>();


        if (_healTimer >= _healSpeed)
        {
            _healthHandler.HealHP(_healAmount);
            _healTimer = 0;
        }
        else
        {
            _healTimer += Time.deltaTime;
        }
    }
}
