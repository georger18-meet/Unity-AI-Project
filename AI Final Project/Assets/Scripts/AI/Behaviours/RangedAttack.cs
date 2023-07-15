using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : AttackBase
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireForce;
    [SerializeField] private float _fireRate;
    private float _fireTimer;


    // Update is called once per frame
    void Update()
    {
        if (AttackEnabled)
        {
            if (_fireTimer >= _fireRate)
            {
                GameObject proj = Instantiate(_projectile, _firePoint.position,_firePoint.rotation);

                proj.GetComponent<Rigidbody>().AddForce(proj.transform.forward * _fireForce, ForceMode.Impulse);

                _fireTimer = 0;
            }
            else
            {
                _fireTimer += Time.deltaTime;
            }
        }
        else
        {
            _fireTimer = 0;
        }
    }
}
