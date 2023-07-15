using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesHolder : MonoBehaviour
{
    [SerializeField] private List<StateInstance> _stateInstances = new List<StateInstance>();

    public Dictionary<Enum, State> StatesDict = new Dictionary<Enum, State>();

    public List<StateInstance> StateInstances { get => _stateInstances; }

    public State GetStateInDict(AllStates stateEnum)
    {
        StatesDict.TryGetValue(stateEnum, out State state);
        return state;
    }

    public void SetupStateInstances()
    {
        foreach (StateInstance instance in _stateInstances)
        {
            instance.SetupInstance(this);
        }
    }

    private void Awake()
    {
        SetupStateInstances();
    }

    public StatesHolder()
    {
        StatesDict.Add(AllStates.Idle, new IdleState());
        StatesDict.Add(AllStates.Chase, new ChaseState());
        StatesDict.Add(AllStates.Attack, new AttackState());
        StatesDict.Add(AllStates.Patrol, new PatrolState());
    }
}
