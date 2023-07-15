using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceFighterAI : CharacterAI
{
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
                    StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = true;
                }
            }
            else
            {
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Idle).FirstCondition = false;
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).FirstCondition = false;
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Chase).SecondCondition = false;
                StateManagerRef.GetStatesHolder.GetStateInDict(AllStates.Attack).FirstCondition = false;
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
        }
    }
}
