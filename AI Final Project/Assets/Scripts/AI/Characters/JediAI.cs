using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JediAI : CharacterAI
{
    public GameObject ShieldObj;
    [SerializeField] private float _toShieldMaxTime = 5;
    private float _toShieldTime;
    private float _timer;

    private void Awake()
    {
        ShieldObj.SetActive(false);
    }

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
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Idle).FirstCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = false;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = false;
                }
                else
                {
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Idle).FirstCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = true;
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = true;
                    //StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = true;
                    ShieldSwitchHandle();
                }
            }
            else
            {
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
            ResetToggleBools();
        }
    }

    private void ShieldSwitchHandle()
    {
        if (_timer > _toShieldTime)
        {
            _timer = 0;
            ToggleBools();
        }
        else
        {
            if (_timer == 0)
            {
                _toShieldTime = Random.Range(_toShieldMaxTime / 2, _toShieldMaxTime);
            }
            _timer += Time.deltaTime;
        }
    }

    private void ToggleBools()
    {
        if (StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition)
        {
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).SecondCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Shield).FirstCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Shield).SecondCondition = true;
        }
        else
        {
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).SecondCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Shield).FirstCondition = true;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Shield).SecondCondition = false;
        }
    }

    private void ResetToggleBools()
    {
        if (StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Shield).SecondCondition)
        {
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).SecondCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Shield).FirstCondition = false;
            StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Shield).SecondCondition = false;
        }
    }
}
