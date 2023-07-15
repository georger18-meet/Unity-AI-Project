using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public AllStates DisplayState;
    [SerializeField] private StatesHolder _statesHolder;
    State _currentState;

    public StatesHolder GetStatesHolder { get => _statesHolder; }


    // Start is called before the first frame update
    void Start()
    {
        _currentState = _statesHolder.GetStateInDict(AllStates.Idle);
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
