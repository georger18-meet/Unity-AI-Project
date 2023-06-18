using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract AllStates ThisState { get; }

    public abstract void EnterState(StateManager manager);
    public abstract void UpdateState(StateManager manager);
}

public enum AllStates
{
    Idle,
    Chase,
    Attack
}
