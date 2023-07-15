using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTestAI : CharacterAI
{
    [SerializeField] private RangedAttack _rangedAttackRef;

    private void Update()
    {
        Run();
        _rangedAttackRef.FireEnabled = Attacking;
    }
}
