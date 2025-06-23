using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class CollisionChecker : MonoBehaviour
{
    [Header("Cấu hình kiểm tra va chạm")]
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float ceilingCheckDistance = 0.05f;
    [SerializeField] private float wallCheckDistance = 0.2f;
    [SerializeField] private float cliffCheckDistance = 0.5f;

    private CapsuleCollider2D col;
    private ContactFilter2D filter;
    private RaycastHit2D[] hits = new RaycastHit2D[5];

    public Vector2 Direction { get; set; } = Vector2.right;

    public bool IsGrounded { get; private set; }
    public bool IsTouchingCeiling { get; private set; }
    public bool IsTouchingWall { get; private set; }
    public bool IsNearCliff { get; private set; }

    private void Awake()
    {
        col = GetComponent<CapsuleCollider2D>();
        filter.useLayerMask = true;
        filter.layerMask = collisionLayer;
        filter.useTriggers = false;
    }

    public void Check()
    {
        // Ground check
        IsGrounded = col.Cast(Vector2.down, filter, hits, groundCheckDistance) > 0;

        // Ceiling check
        IsTouchingCeiling = col.Cast(Vector2.up, filter, hits, ceilingCheckDistance) > 0;

        // Wall check (dựa trên hướng di chuyển)
        IsTouchingWall = col.Cast(Direction.normalized, filter, hits, wallCheckDistance) > 0;

        // Cliff check
        Vector2 origin = (Vector2)col.bounds.center + Direction.normalized * (col.bounds.size.x / 2 + cliffCheckDistance);
        IsNearCliff = !Physics2D.Raycast(origin, Vector2.down, 1f, collisionLayer) && IsGrounded;
    }
}
