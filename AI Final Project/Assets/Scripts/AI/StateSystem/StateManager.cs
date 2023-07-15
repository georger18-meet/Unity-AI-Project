using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private StatesHolder _statesHolder;
    State _currentState;
    private AllStates DisplayState;

    public StatesHolder GetStatesHolder { get => _statesHolder; }


    // Start is called before the first frame update
    void Start()
    {
        _currentState = _statesHolder.GetStateInDict(_statesHolder.StateInstances[0].CurrentState);
        _currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState(this);
        DisplayState = _currentState.ThisState;
    }


    public void SwitchState(State state)
    {
        _currentState = state;
        _currentState.EnterState(this);
    }
}
