using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(StateManager))]
public class CharacterAI : MonoBehaviour
{
    private HealthHandler _healthHandler;

    [Header("Movement")]
    public Vector3 Direction;
    public float MaxSpeed = 5;
    public float TurnDist = 1;
    public float AngularSpeed = 3;
    private Vector3 _movingVelocity;
    private float _currentSpeed;
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
    public bool CanFly = false;
    public bool UseTarget = true;
    public Transform Target;
    public Vector3 Destination;
    public float StoppingDistance = 1f;
    public const float PathRefreshMoveThreshold = 0.5f;
    public const float PathMinRefreshTime = 0.2f;
    private float _targetDistance;
    private float _targetDistanceNoHeight;
    private bool _reachedDestination;
    private Path _path;

    [Header("Gizmos")]
    public bool ShowGrounded;
    public bool ShowRange;
    public bool ShowPath;

    [Header("States")]
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _detectionRange = 10;
    [SerializeField] private float _attackingRange = 2;
    bool _detected;
    bool _attacking;
    private StateManager _stateManager;

    [Header("Opponent Related")]
    [SerializeField] private Collider _charCollider;
    [SerializeField] private UserType _userType;


    public float CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
    public float TargetDistance { get => _targetDistance; }
    public float DetectionRange { get => _detectionRange; }
    public float AttackingRange { get => _attackingRange; }
    public bool Detected { get => _detected; set => _detected = value; }
    public bool Attacking { get => _attacking; set => _attacking = value; }
    public StateManager StateManagerRef { get => _stateManager; }
    internal UserType GetUserType { get => _userType; }


    private void Start()
    {
        _healthHandler = GetComponent<HealthHandler>();
        _healthHandler.OnDeathOccured += _healthHandler_OnDeathOccured;

        _currentSpeed = MaxSpeed;

        _stateManager = GetComponent<StateManager>();

        if (UseTarget && Target != null)
        {
            Destination = Target.position;
        }
        else if (UseTarget && Target == null)
        {
            Destination = transform.position;
        }
        StartCoroutine(UpdatePath());
    }

    private void _healthHandler_OnDeathOccured(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        Run();
    }

    public void Run()
    {
        if (UseTarget && Target != null) Destination = Target.position;
        CheckRange();
        StateHandler();
    }


    #region PathFinding

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessfull)
    {
        if (pathSuccessfull && waypoints.Length != 0)
        {
            _path = new Path(waypoints, transform.position, TurnDist, StoppingDistance);
            StopFollowingPath();
            StartFollowingPath();
            //Debug.Log(gameObject.name + ": Path Found");
        }
        else
        {
            //Debug.Log(gameObject.name + ": Invalid Path");
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.5f) yield return new WaitForSeconds(0.5f);
        PathRequestManager.RequestPath(new PathRequest(transform.position, Destination, CanFly, OnPathFound));

        float sqrMoveThreshold = PathRefreshMoveThreshold * PathRefreshMoveThreshold;
        Vector3 targetPosOld = Destination;

        while (true)
        {
            yield return new WaitForSeconds(PathMinRefreshTime);
            if ((Destination - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, Destination, CanFly, OnPathFound));
                targetPosOld = Destination;

            }
        }
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(_path.LookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (_path.TurnBounds[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == _path.FinishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                if (pathIndex >= _path.SlowDownIndex && StoppingDistance > 0)
                {
                    speedPercent = Mathf.Clamp01(_path.TurnBounds[_path.FinishLineIndex].DistanceFromPoint(pos2D) / StoppingDistance);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }

                Quaternion targetRot = Quaternion.LookRotation(_path.LookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, AngularSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * _currentSpeed * speedPercent * Time.deltaTime, Space.Self);
            }

            yield return null;
        }
    }

    public void StopFollowingPath()
    {
        StopCoroutine("FollowPath");
    }

    public void StartFollowingPath()
    {
        StartCoroutine("FollowPath");
    }

    #endregion


    private void CheckRange()
    {
        if (UseTarget)
        {
            if (!Target)
            {
                Target = null;
                Collider[] targets = Physics.OverlapSphere(transform.position, _detectionRange, _targetLayer);
                List<CharacterAI> trueTargets = new List<CharacterAI>();
                if (targets.Length > 0)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        if (targets[i] != _charCollider)
                        {
                            if (targets[i].TryGetComponent(out CharacterAI targetAI))
                            {
                                if (targetAI.GetUserType != _userType)
                                {
                                    trueTargets.Add(targetAI);
                                }
                            }
                        }
                    }

                    if (trueTargets.Count > 0) Target = trueTargets[Random.Range(0, trueTargets.Count)].transform;
                }
            }
            else
            {
                MathDistances();
                if (_targetDistance > _detectionRange)
                {
                    //if (Target.TryGetComponent(out CharacterAI targetAI))
                    //{
                    //    targetAI.Targeted = false;
                    //}
                    Target = null;
                }
            }
        }
    }

    public virtual void StateHandler()
    {
        // Override Within the Inherited Character Script
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

    private void MathDistances()
    {
        Vector3 posYZero = transform.position;
        posYZero.y = 0;
        Vector3 destYZero = Target.position;
        destYZero.y = 0;

        _targetDistance = Vector3.Distance(transform.position, Target.position);
        _targetDistanceNoHeight = Vector3.Distance(posYZero, destYZero);
    }

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

        if (ShowRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackingRange);
        }

        if (ShowPath)
        {
            if (_path != null)
            {
                _path.DrawWithGizmos();
            }
        }
    }
}
