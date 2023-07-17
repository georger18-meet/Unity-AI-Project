using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AttackBase
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Damager _meleeWeaponDamager;
    [SerializeField] private float _attackStart = 0.2f;
    [SerializeField] private float _attackEnd = 1.0f;
    private float _attackTimer;

    // Update is called once per frame
    void Update()
    {
        if (AttackEnabled)
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attackStart && _attackTimer < _attackEnd)
            {
                _meleeWeaponDamager.CanDamage = true;
                _animator.SetBool("Attacking", true);
            }
            else if (_attackTimer >= _attackEnd)
            {
                _meleeWeaponDamager.CanDamage = false;
                _animator.SetBool("Attacking", false);
                _attackTimer = 0;
            }
        }
        else
        {
            _animator.SetBool("Attacking", false);
            _attackTimer = 0;
        }
    }
}
