using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneTrooperAI : CharacterAI
{
    [SerializeField] private float _switchAttackModeMaxTime = 5;
    private float _switchTime;
    private float _timer;
    public bool _attackSwitched;

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
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Patrol).FirstCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = false;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = false;
                }
                else
                {
                    AttackSwitchHandle();
                }
            }
            else
            {
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Patrol).FirstCondition = false;
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = false;
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = false;
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = false;
            }
        }
        else if (StateManagerRef && UseTarget && !Target)
        {
            Detected = false;
            Attacking = false;

            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Patrol).FirstCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = false;
        }
    }

    private void AttackSwitchHandle()
    {
        if (_timer > _switchTime)
        {
            _timer = 0;
            _attackSwitched = !_attackSwitched;
        }
        else
        {
            if (_timer == 0)
            {
                _switchTime = Random.Range(0, _switchAttackModeMaxTime);
            }
            _timer += Time.deltaTime;
        }


        if (!_attackSwitched)
        {
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Patrol).FirstCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).SecondCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.AOE).FirstCondition = false;
        }
        else
        {
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Patrol).FirstCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).SecondCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.AOE).FirstCondition = true;
        }
    }
}
