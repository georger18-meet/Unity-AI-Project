using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public AllStates DisplayState;
    State _currentState;
    public IdleState _idleState = new IdleState();
    public ChaseState _chaseState = new ChaseState();
    public AttackState _attackState = new AttackState();


    // Start is called before the first frame update
    void Start()
    {
        _currentState = _idleState;
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
