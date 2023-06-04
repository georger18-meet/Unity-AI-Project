using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTestAI : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 Direction;
    public float Speed;
    public float AngularSpeed;
    private Vector3 _movingVelocity;
    private bool _isMoving;
    private bool _canMove;

    [Header("Gravity")]
    public bool UseGravity;
    public float GravityAcceleration = -10f;
    public Vector3 GroundCheckOffset;
    public Vector3 GroundedBoxSize = Vector3.one;
    public LayerMask GroundLayer;
    private bool _isGrounded;
    private Vector3 _gravityVelocityEffect;

    [Header("Navigation")]
    public bool UseNav = true;
    public Vector3 Destination;
    public float StoppingDistance = 0.1f;
    private float _targetDistance;
    private float _targetDistanceNoHeight;
    private bool _reachedDestination;
    private Vector3[] _path;
    private int _targetIndex;

    [Header("Gizmos")]
    public bool ShowGrounded;
    public bool ShowPath;


    // Start is called before the first frame update
    void Start()
    {
        PathRequestManager.RequestPath(transform.position, Destination, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessfull)
    {
        if (pathSuccessfull && newPath.Length != 0)
        {
            _path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
            Debug.Log(gameObject.name + ": Path Found");
        }
        else
        {
            Debug.Log(gameObject.name + ": Invalid Path");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = _path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                _targetIndex++;
                if (_targetIndex >= _path.Length)
                {
                    yield break;
                }
                currentWaypoint = _path[_targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Speed * Time.deltaTime);
            yield return null;
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (_movingVelocity.magnitude != 0 && _canMove) _isMoving = true;
    //    else _isMoving = false;

    //    GoToDestination();
    //}

    //private void FixedUpdate()
    //{
    //    MovementHandler();
    //    GravityHandler();
    //}


    //private void MovementHandler()
    //{
    //    if (_canMove)
    //    {
    //        if (UseNav)
    //        {
    //            if (transform.position.y > Destination.y + StoppingDistance)
    //            {
    //                Direction.y = -1;
    //            }
    //            else if (transform.position.y < Destination.y - StoppingDistance)
    //            {
    //                Direction.y = 1;
    //            }
    //            else
    //            {
    //                Direction.y = 0;
    //            }
    //        }

    //        Vector3 moveDir = ((transform.right * Direction.x) + (transform.up * Direction.y) + (transform.forward * Direction.z)).normalized;
    //        _movingVelocity = moveDir * (Speed * Time.fixedDeltaTime);
    //        transform.position += _movingVelocity;
    //    }
    //}

    //private void GravityHandler()
    //{
    //    _isGrounded = Physics.CheckBox(transform.position + GroundCheckOffset, GroundedBoxSize / 2, Quaternion.identity, GroundLayer);

    //    if (UseGravity)
    //    {

    //        if (_isGrounded && _gravityVelocityEffect.y <= 0)
    //        {
    //            _gravityVelocityEffect.y = 0;
    //        }
    //        else
    //        {
    //            _gravityVelocityEffect.y += GravityAcceleration * Time.fixedDeltaTime;
    //            transform.position += _gravityVelocityEffect * Time.fixedDeltaTime;
    //        }
    //    }
    //    else
    //    {
    //        _gravityVelocityEffect.y = 0;
    //    }
    //}

    //private void MathDistances()
    //{
    //    Vector3 posYZero = transform.position;
    //    posYZero.y = 0;
    //    Vector3 destYZero = Destination;
    //    destYZero.y = 0;

    //    _targetDistance = Vector3.Distance(transform.position, Destination);
    //    _targetDistanceNoHeight = Vector3.Distance(posYZero, destYZero);
    //}

    //private void GoToDestination()
    //{
    //    if (UseNav)
    //    {
    //        MathDistances();

    //        if (_targetDistance < StoppingDistance)
    //        {
    //            _reachedDestination = true;
    //        }
    //        else
    //        {
    //            _reachedDestination = false;
    //        }

    //        if (!_reachedDestination)
    //        {
    //            FaceTarget();
    //            _canMove = true;
    //        }
    //        else
    //        {
    //            _canMove = false;
    //        }
    //    }
    //    else
    //    {
    //        _canMove = true;
    //    }
    //}

    //private void FaceTarget()
    //{
    //    if (_targetDistanceNoHeight >= StoppingDistance)
    //    {
    //        Vector3 lookPos = (Destination - transform.position).normalized;
    //        lookPos.y = 0;
    //        Quaternion rotation = Quaternion.LookRotation(lookPos);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * AngularSpeed);
    //    }
    //}

    private void OnDrawGizmos()
    {
        if (ShowGrounded)
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (_isGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            Gizmos.DrawCube(transform.position + GroundCheckOffset, GroundedBoxSize);
        }

        if (ShowPath)
        {
            if (_path != null)
            {
                for (int i = _targetIndex; i < _path.Length; i++)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(_path[i], Vector3.one);

                    if (i == _targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, _path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(_path[i - 1], _path[i]);
                    }
                }
            }
        }
    }
}
