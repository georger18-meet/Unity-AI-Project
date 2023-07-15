using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract AllStates ThisState { get; }
    public AllStates PreviousState;
    public AllStates NextState;

    public bool FirstCondition { get; set; }
    public bool SecondCondition { get; set; }

    public abstract void EnterState(StateManager manager);
    public abstract void UpdateState(StateManager manager);
}

public enum AllStates
{
    Idle,
    Chase,
    Attack
}
