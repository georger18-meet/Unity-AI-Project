using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public override AllStates ThisState { get => AllStates.Patrol; }

    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entered Patrol State");

        if (!_characterAIRef)
        {
            _characterAIRef = manager.GetComponent<CharacterAI>();
            _characterTransform = _characterAIRef.transform;
        }

        ResetValues();
    }

    public override void UpdateState(StateManager manager)
    {
        Debug.Log("Updating Patrol State");
        if (FirstCondition)
        {
            ResetValues();
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(NextState));
        }
        else if (SecondCondition)
        {
            ResetValues();
            manager.SwitchState(manager.GetStatesHolder.GetStateInDict(PreviousState));
        }
        Patrolling(manager);
    }


    // Not From Base State Class
    // ------------------------------
    private CharacterAI _characterAIRef;
    private Transform _characterTransform;
    public float PatrolRange = 10;
    public float MaxWaitTime = 5;
    private float _waitTime;
    private float _waitingTimer;
    private Vector3 _startingPoint;
    private Vector3 WalkPoint;
    private bool _walkPointSet;
    private bool _reachedPoint;

    virtual public void Patrolling(StateManager manager)
    {
        _characterAIRef.CurrentSpeed = _characterAIRef.MaxSpeed / 2;
        if (!_walkPointSet)
        {
            SearchWalkPoint();
            _walkPointSet = true;
        }

        if (_walkPointSet && !_reachedPoint)
            _characterAIRef.Destination = WalkPoint;

        Vector3 distanceToWalkPoint = _characterTransform.position - WalkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            if (!_reachedPoint)
            {
                _waitTime = Random.Range(0, MaxWaitTime);
                _waitingTimer = 0;
                _walkPointSet = false;
                _reachedPoint = true;
            }
        }
        else if (!_reachedPoint && _waitingTimer >= MaxWaitTime)
        {
            _waitTime = Random.Range(0, MaxWaitTime);
            _waitingTimer = 0;
            _walkPointSet = false;
            _reachedPoint = true;
        }
        if (_reachedPoint)
        {
            WaitInPlace();
        }
        else
        {
            _waitingTimer += Time.deltaTime;
        }
    }

    private void ResetValues()
    {
        _waitingTimer = 0;
        _reachedPoint = false;
        _walkPointSet = false;
        _startingPoint = _characterTransform.position;
        _characterAIRef.CurrentSpeed = _characterAIRef.MaxSpeed;
        _characterAIRef.StopFollowingPath();
    }

    private void WaitInPlace()
    {
        if (_waitingTimer >= _waitTime)
        {
            ResetValues();
        }
        else
        {
            _waitingTimer += Time.deltaTime;
            _characterAIRef.StopFollowingPath();
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-PatrolRange, PatrolRange);
        float randomX = Random.Range(-PatrolRange, PatrolRange);

        WalkPoint = new Vector3(_startingPoint.x + randomX, _startingPoint.y, _startingPoint.z + randomZ);
    }
}
