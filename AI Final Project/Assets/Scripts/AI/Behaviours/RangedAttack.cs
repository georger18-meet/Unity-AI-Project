using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RangedAttack : AttackBase
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireForce;
    [SerializeField] private float _fireRate;
    private float _fireTimer;

    [Range(0, 100)]
    [SerializeField] private int _accuracy = 100;
    [Range(0, 90)]
    [SerializeField] private float _maxAccuracyOffset = 0;
    private float _finalAccuracyOffset;

    // Update is called once per frame
    void Update()
    {
        if (AttackEnabled)
        {
            if (_fireTimer >= _fireRate)
            {
                SetTargetDestination();

                GameObject proj = Instantiate(_projectile, _firePoint.position, _firePoint.rotation);

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

    void SetTargetDestination()
    {
        _finalAccuracyOffset = _maxAccuracyOffset / 100 * (100 - _accuracy);

        Vector3 tempOffset = new Vector3(0, Random.Range(-_finalAccuracyOffset, _finalAccuracyOffset), 0);
        _firePoint.localRotation = Quaternion.Euler(tempOffset);
    }
}
