using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class EnemyVisionMesh : MonoBehaviour
{
    [Header("Vision Shape")]
    [SerializeField] private float viewDistance = 6f;

    [Range(1f, 180f)]
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private int rayCount = 60;

    [Header("Obstacle")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Position")]
    [SerializeField] private float forwardOffset = 0.5f;

    private Mesh mesh;

    private void Awake()
    {
        transform.localPosition = new Vector3(
            forwardOffset,
            0f,
            transform.localPosition.z
        );
        mesh = new Mesh();
        mesh.name = "EnemyVisionMesh";


        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        BuildVisionMesh();
    }

    private void BuildVisionMesh()
    {
        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        Vector2[] uvs = new Vector2[rayCount + 2];

        vertices[0] = Vector3.zero;
        uvs[0] = new Vector2(0f, 0.5f);

        float startAngle = -viewAngle / 2f;
        float angleStep = viewAngle / rayCount;

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle + angleStep * i;
            float angleRad = angle * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(
                Mathf.Cos(angleRad),
                Mathf.Sin(angleRad)
            );
            Vector2 worldDirection = transform.TransformDirection(direction);
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                worldDirection,
                viewDistance,
                obstacleLayer
            );
            float distance = viewDistance;
            if (hit.collider != null)
            {
                distance = hit.distance;
            }
            vertices[i + 1] = direction * distance;
            float normalizedY = (float)i / rayCount;
            uvs[i + 1] = new Vector2(1f, normalizedY);

            if (i < rayCount)
            {
                int triangleIndex = i * 3;

                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = i + 1;
                triangles[triangleIndex + 2] = i + 2;
            }
        }
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateBounds();
    }
}
