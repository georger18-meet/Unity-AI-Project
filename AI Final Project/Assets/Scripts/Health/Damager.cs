using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Who can get Damaged
enum UserType
{
    Jedi,
    Clone,
    Resistance
}


public class Damager : MonoBehaviour
{
    // Variables
    // --------------------
    [Header("Damage")]
    [SerializeField] private UserType _userType;
    [SerializeField] private int _damageAmount;
    public bool CanDamage = false;


    // Properties
    // --------------------
    internal UserType GetUserType { get { return _userType; } }

    public int DamageAmount { get { return _damageAmount; } }

}
