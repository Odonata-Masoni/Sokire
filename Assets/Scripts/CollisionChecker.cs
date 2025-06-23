using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [Header("Cấu hình kiểm tra va chạm")]
    [SerializeField] public LayerMask collisionLayer;
    [SerializeField] public float groundDistance = 0.2f;
    [SerializeField] public float wallDistance = 0.4f; // Tăng lên 0.4 để phát hiện sớm hơn
    [SerializeField] public float ceilingDistance = 0.05f;
    [SerializeField] public float cliffDistance = 0.5f;

    private CapsuleCollider2D touchingCol;
    private Animator animator;
    private RaycastHit2D[] hits = new RaycastHit2D[5];
    private ContactFilter2D castFilter;

    [SerializeField] private bool _isGrounded;
    public bool IsGrounded
    {
        get => _isGrounded;
        private set { _isGrounded = value; animator?.SetBool(AnimationStrings.isGrounded, value); }
    }

    [SerializeField] private bool _isTouchingCeiling;
    public bool IsTouchingCeiling
    {
        get => _isTouchingCeiling;
        private set { _isTouchingCeiling = value; animator?.SetBool(AnimationStrings.isTouchingCeiling, value); }
    }

    [SerializeField] private bool _isTouchingWall;
    public bool IsTouchingWall
    {
        get => _isTouchingWall;
        private set { _isTouchingWall = value; animator?.SetBool(AnimationStrings.isTouchingWall, value); }
    }

    [SerializeField] private bool _isNearCliff;
    public bool IsNearCliff
    {
        get => _isNearCliff;
        private set { _isNearCliff = value; animator?.SetBool(AnimationStrings.isNearCliff, value); }
    }

    private Vector2 wallCheckDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();

        castFilter.useLayerMask = true;
        castFilter.SetLayerMask(collisionLayer);
    }

    public void CheckCollisions()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, hits, groundDistance) > 0;
        IsTouchingCeiling = touchingCol.Cast(Vector2.up, castFilter, hits, ceilingDistance) > 0;
        IsTouchingWall = touchingCol.Cast(wallCheckDirection, castFilter, hits, wallDistance) > 0;

        Vector2 checkCliffPos = (Vector2)touchingCol.bounds.center + wallCheckDirection * (touchingCol.bounds.size.x / 2 + cliffDistance);
        IsNearCliff = !Physics2D.Raycast(checkCliffPos, Vector2.down, 2f, collisionLayer) && !IsGrounded;
        Debug.Log("IsGrounded: " + IsGrounded + " | IsTouchingWall: " + IsTouchingWall + " | WallDirection: " + wallCheckDirection);
    }
}