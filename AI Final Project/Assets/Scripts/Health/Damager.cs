using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Who can get Damaged
enum CanAffect
{
    Home,
    Away,
    Both
}


public class Damager : MonoBehaviour
{
    // Variables
    // --------------------
    [Header("Damage")]
    [SerializeField] private CanAffect _canAffect;
    [SerializeField] private int _damageAmount;
    public bool CanDamage = false;


    // Properties
    // --------------------
    internal CanAffect CanAffect { get { return _canAffect; } }

    public int DamageAmount { get { return _damageAmount; } }

}
