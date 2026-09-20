using UnityEngine;

public class EnemyFacing : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 720f;

    private Vector2 facingDirection = Vector2.right;
    public Vector2 FacingDirection
    {
        get
        {
            return facingDirection;
        }
    }
    public void SetFacingDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }
        facingDirection = direction.normalized;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        float targetAngle = Mathf.Atan2(
            facingDirection.y,
            facingDirection.x
        ) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        if (transform != null)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
