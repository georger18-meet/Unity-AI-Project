using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTestAI : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 Direction;
    public float Speed;
    private Vector3 _movingVelocity;
    private bool _isMoving;

    [Header("Gravity")]
    public bool UseGravity;
    public float GravityAcceleration = -10f;
    public Vector3 GroundCheckOffset;
    public Vector3 GroundedBoxSize = Vector3.one;
    public LayerMask GroundLayer;
    private bool _isGrounded;
    private Vector3 _gravityVelocityEffect;


    private void OnValidate()
    {
        Direction = Direction.normalized;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Direction = Direction.normalized;

        if (_movingVelocity.magnitude != 0) _isMoving = true;
        else _isMoving = false;
    }

    private void FixedUpdate()
    {
        MovementHandler();
        GravityHandler();
    }


    private void MovementHandler()
    {
        Vector3 moveDir = ((transform.right * Direction.x) + (transform.up * Direction.y) + (transform.forward * Direction.z)).normalized;
        _movingVelocity = moveDir * (Speed * Time.fixedDeltaTime);
        transform.position += _movingVelocity;
    }

    private void GravityHandler()
    {
        if (UseGravity)
        {
            _isGrounded = Physics.CheckBox(transform.position + GroundCheckOffset, GroundedBoxSize / 2, Quaternion.identity, GroundLayer);

            if (_isGrounded && _gravityVelocityEffect.y <= 0)
            {
                _gravityVelocityEffect.y = 0;
            }
            else
            {
                _gravityVelocityEffect.y += GravityAcceleration * Time.fixedDeltaTime;
                transform.position += _gravityVelocityEffect * Time.fixedDeltaTime;
            }
        }
        else
        {
            _gravityVelocityEffect.y = 0;
        }
    }


    private void OnDrawGizmos()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (_isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        Gizmos.DrawCube(transform.position + GroundCheckOffset, GroundedBoxSize);
    }
}
