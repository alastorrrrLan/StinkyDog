using UnityEngine;
using Pathfinding;

// Autinatically add seeker
[RequireComponent(typeof(Seeker))]
public class Enemy : MonoBehaviour
{
    [Header("Patrol Point")]
    [SerializeField] private PatrolRoute patrolRoute;

    [Header("Movement Settings")]

    // move speed
    [SerializeField] private float moveSpeed = 2f;
    // arrival distance
    [SerializeField] private float waypointDistance = 0.1f;

    [Header("Patrol")]
    [SerializeField]private float waitTime = 1f;

    private int patrolDirection = 1;
    private int currentPatrolIndex = 0;
    private float waitTimer;
    private bool waiting;
    private bool isPatrolling = true;

    private Seeker seeker;
    private EnemyFacing enemyFacing;
    private Path currentPath;
    private int currentWaypointIndex;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        enemyFacing = GetComponent<EnemyFacing>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (patrolRoute.PointCount == 0) return;
        GoToPatrolPoint();
    }

    private void Update()
    {
        if (waiting) {
            HandleWaiting();
            return;
        }
        FollowPath();
    }

    private void GoToPatrolPoint()
    {
        Transform target = patrolRoute.GetPoint(currentPatrolIndex);
        MoveToPos(target.position);
    }

    private void FollowPath()
    {
        if (currentPath == null) return;
        if (currentWaypointIndex >= currentPath.vectorPath.Count)
        {
            currentPath = null;

            if (isPatrolling)
            {
                ArriveAtPatrolPoint();
            }
            else
            {
                ResumePatrol();
            }
            return;
        }

        Vector3 targetPoint =
            currentPath.vectorPath[currentWaypointIndex];

        targetPoint.z = transform.position.z;
        float distance = Vector2.Distance(
            transform.position,
            targetPoint
        );
        if (distance <= waypointDistance)
        {
            currentWaypointIndex++;
            return;
        }
        
        Vector2 moveDirection = targetPoint - transform.position;
        if (enemyFacing != null)
        {
            enemyFacing.SetFacingDirection(moveDirection);
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint,
            moveSpeed * Time.deltaTime
        );
    }

    private void ArriveAtPatrolPoint()
    {
        currentPath = null;
        waiting = true;
        waitTimer = 0;
    }

    private void HandleWaiting()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer < waitTime) return;

        waiting = false;
        SelectNextPatrolPoint();
        GoToPatrolPoint();
    }

    private void SelectNextPatrolPoint()
    {
        int pointCount = patrolRoute.PointCount;
        if (pointCount <= 1) return;

        currentPatrolIndex += patrolDirection;

        if (currentPatrolIndex >= pointCount)
        {
            patrolDirection = -1;
            currentPatrolIndex = pointCount - 2;
        } else if (currentPatrolIndex < 0)
        {
            patrolDirection = -1;
            currentPatrolIndex = 1;

        }
    }

    private void OnPathComplete(Path path)
    {
        if (path.error) return;
        currentPath = path;
        currentWaypointIndex = 0;
    }

    public void MoveToPos(Vector2 targetPosition)
    {
        seeker.StartPath(
            transform.position,
            targetPosition,
            OnPathComplete
        );
    }

    public void MoveToSound(Vector2 soundOrigin)
    {
        isPatrolling = false;
        waiting = false;
        MoveToPos(soundOrigin);
    }

    public void ResumePatrol()
    {
        isPatrolling = true;
        waiting = false;
        GoToPatrolPoint();
    }
}
