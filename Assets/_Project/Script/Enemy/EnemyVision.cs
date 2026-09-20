using UnityEngine;
public enum VisionZone
{
    None,
    Far,
    Near
}

public class EnemyVision : MonoBehaviour
{
    [Header("Vision")]
    [SerializeField] private float nearDistance = 2.5f;
    [SerializeField] private float farDistance = 6f;
    [SerializeField] private float viewDistance = 6f;

    [Range(0f, 360f)]
    [SerializeField] private float viewAngle = 90f;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private EnemyFacing enemyFacing;
    [SerializeField] private Enemy enemy;

    [Header("Layers")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Warning UI")]
    [SerializeField] private GameObject warningBorder;

    private VisionZone currentZone = VisionZone.None;
    private Vector2 currentPlayerPos;

    private float originalMoveSpeed;

    public VisionZone CurrentZone
    {
        get
        {
            return currentZone;
        }
    }

    private bool canSeePlayer;

    public bool CanSeePlayer
    {
        get
        {
            return canSeePlayer;
        }
    }

    private void Start()
    {
        originalMoveSpeed = enemy.moveSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckPlayer();
    }

    private void CheckPlayer()
    {
        if (player == null || enemyFacing == null) return;

        Vector2 toPlayer = player.position - transform.position;
        currentPlayerPos = player.position;

        float distanceToPlayer = toPlayer.magnitude;
        
        if (distanceToPlayer > farDistance)
        {
            SetVisionZone(VisionZone.None);
            return;
        }

        Vector2 directionToPlayer = toPlayer.normalized;

        Vector2 forward = enemyFacing.FacingDirection;

        float angleToPlayer = Vector2.Angle(forward, directionToPlayer);
        if (angleToPlayer > viewAngle / 2f)
        {
            SetVisionZone(VisionZone.None);
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToPlayer,
            distanceToPlayer,
            obstacleLayer
        );
        if (hit.collider != null)
        {
            SetVisionZone(VisionZone.None);
            return;
        }

        if (distanceToPlayer <= nearDistance)
        {
            SetVisionZone(VisionZone.Near);
            return;
        }
        SetVisionZone(VisionZone.Far);
    }

    private void SetVisionZone(VisionZone newZone)
    {
        if (currentZone == newZone) return;

        currentZone = newZone;
        switch (currentZone)
        {
            case VisionZone.None:
            if (warningBorder != null) warningBorder.SetActive(false);
            enemy.moveSpeed = originalMoveSpeed;
            break;
            case VisionZone.Far:
            if (enemy != null) enemy.MoveToPos(currentPlayerPos);
            if (warningBorder != null) warningBorder.SetActive(true);
            enemy.moveSpeed = originalMoveSpeed * 2;
            break;
            case VisionZone.Near:
            if (warningBorder != null) warningBorder.SetActive(true);
            // TODO: gameover
            // if (GameManager.instance != null)
            // {
            //     GameManager.instance.GameOver();
            // }
            break;
        }

    }
}
