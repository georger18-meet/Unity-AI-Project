using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceFighterAI : CharacterAI
{
    [SerializeField] private float _toDodgeMaxTime = 10;
    private float _toDodgeTime;
    private float _timer;

    private void Update()
    {
        Run();
    }

    public override void StateHandler()
    {
        if (StateManagerRef && UseTarget && Target)
        {
            if (TargetDistance <= AttackingRange)
            {
                Attacking = true;
                StopFollowingPath();
            }
            else if (TargetDistance <= DetectionRange)
            {
                Detected = true;
                Attacking = false;
            }
            else
            {
                Detected = false;
                Attacking = false;
            }


            if (Detected)
            {
                if (!Attacking)
                {
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Idle).SecondCondition = false;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Heal).SecondCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Idle).FirstCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = false;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = false;
                }
                else
                {
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Idle).SecondCondition = false;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Heal).SecondCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Idle).FirstCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = true;
                    AttackSwitchHandle();
                }
            }
            else
            {
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Idle).SecondCondition = false;
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Heal).SecondCondition = true;
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Idle).FirstCondition = false;
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = false;
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = false;
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = false;
                ResetToggleBools();
            }
        }
        else if (StateManagerRef && UseTarget && !Target)
        {
            Detected = false;
            Attacking = false;

            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Idle).FirstCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Idle).SecondCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Heal).SecondCondition = false;
            ResetToggleBools();
        }
    }

    private void AttackSwitchHandle()
    {
        if (_timer > _toDodgeTime)
        {
            _timer = 0;
            ToggleBools();
        }
        else
        {
            if (_timer == 0)
            {
                _toDodgeTime = Random.Range(_toDodgeMaxTime/2, _toDodgeMaxTime);
            }
            _timer += Time.deltaTime;
        }
    }

    private void ToggleBools()
    {
        if (!StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).SecondCondition)
        {
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).SecondCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Dodge).FirstCondition = false;
        }
        else
        {
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).SecondCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Dodge).FirstCondition = true;
        }
    }

    private void ResetToggleBools()
    {
        if (!StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Dodge).FirstCondition)
        {
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).SecondCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = false;
        }
    }
}
