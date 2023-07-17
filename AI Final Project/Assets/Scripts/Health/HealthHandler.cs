using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class HealthHandler : MonoBehaviour
{
    public event EventHandler OnDeathOccured;
    public event EventHandler OnDamageOccured;

    public HealthSystem _healthSystem;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;
    [SerializeField] private bool _invulnerable = false;
    private CharacterAI _characterAI;

    public bool Invulnerable { get => _invulnerable; set => _invulnerable = value; }


    private void Awake()
    {
        _healthSystem = new HealthSystem(_maxHP);
        _healthSystem.OnDeath += _healthSystem_OnDeath;
        _healthSystem.OnDamaged += _healthSystem_OnDamaged;
        _characterAI = GetComponent<CharacterAI>();
    }

    private void _healthSystem_OnDamaged(object sender, EventArgs e)
    {
        if (OnDamageOccured != null) OnDamageOccured(this, EventArgs.Empty);
    }

    private void _healthSystem_OnDeath(object sender, System.EventArgs e)
    {
        if (OnDeathOccured != null) OnDeathOccured(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        _currentHP = _healthSystem.CurrentHealth;
    }

    public void HealHP(int hp)
    {
        _healthSystem.Heal(hp);
    }

    private void TakeDamage(GameObject damagerObj)
    {
        // Get Damage Info from Damager GameObject
        Damager tempDamager = damagerObj.GetComponent<Damager>();

        if (tempDamager.CanDamage)
        {
            // Check if GameObject can be Affected by Damager
            if (_characterAI.HomeTeam && tempDamager.CanAffect == CanAffect.Home || !_characterAI.HomeTeam && tempDamager.CanAffect == CanAffect.Away || tempDamager.CanAffect == CanAffect.Both)
            {
                // If Damager is One Hit
                _healthSystem.Damage(tempDamager.DamageAmount);
            }
        }
    }



    // Collision Handling
    // --------------------
    private void OnTriggerEnter(Collider other)
    {
        if (!_invulnerable)
        {
            if (other.CompareTag("Damager"))
            {
                TakeDamage(other.gameObject);
            }
        }
    }
}