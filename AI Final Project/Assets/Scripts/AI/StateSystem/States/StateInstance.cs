using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateInstance : MonoBehaviour
{
    public State StateRef;
    private StatesHolder _statesHolder;

    public AllStates CurrentState;
    public AllStates PreviousState;
    public AllStates NextState;

    public void SetupInstance(StatesHolder holder)
    {
        _statesHolder = holder; 
        StateRef = _statesHolder.GetStateInDict(CurrentState);
        StateRef.PreviousState = PreviousState;
        StateRef.NextState = NextState;
    }
}
