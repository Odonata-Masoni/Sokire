using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField] public LayerMask collisionLayer; // Layer của các vật cản
    [SerializeField] public float groundDistance = 0.2f; // Khoảng cách kiểm tra đất
    [SerializeField] public float wallDistance = 0.5f;   // Khoảng cách kiểm tra tường
    [SerializeField] public float ceilingDistance = 0.2f; // Khoảng cách kiểm tra trần
    [SerializeField] public float cliffDistance = 0.5f;  // Khoảng cách kiểm tra mép vực

    private CapsuleCollider2D touchingCol;
    private Animator animator;
    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] wallHits = new RaycastHit2D[5];
    private RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    private RaycastHit2D[] cliffHits = new RaycastHit2D[5];

    private ContactFilter2D castFilter;

    [SerializeField] private bool _isGrounded;
    public bool IsGrounded
    {
        get => _isGrounded;
        private set
        {
            _isGrounded = value;
            animator?.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    [SerializeField] private bool _isTouchingCeiling;
    public bool IsTouchingCeiling
    {
        get => _isTouchingCeiling;
        private set
        {
            _isTouchingCeiling = value;
            animator?.SetBool(AnimationStrings.isTouchingCeiling, value);
        }
    }

    [SerializeField] private bool _isTouchingWall;
    private Vector2 wallCheckDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    public bool IsTouchingWall
    {
        get => _isTouchingWall;
        private set
        {
            _isTouchingWall = value;
            animator?.SetBool(AnimationStrings.isTouchingWall, value);
        }
    }

    [SerializeField] private bool _isNearCliff;
    public bool IsNearCliff
    {
        get => _isNearCliff;
        private set
        {
            _isNearCliff = value;
            animator?.SetBool(AnimationStrings.isNearCliff, value);
        }
    }

    void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        if (touchingCol == null)
        {
            Debug.LogError("CapsuleCollider2D not found on " + gameObject.name);
        }

        // Cấu hình ContactFilter2D
        castFilter.useLayerMask = true;
        castFilter.SetLayerMask(collisionLayer);
    }

    void FixedUpdate()
    {
        CheckCollisions();
    }

    public void CheckCollisions()
    {
        // Kiểm tra đất
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;

        // Kiểm tra trần
        IsTouchingCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;

        // Kiểm tra tường
        IsTouchingWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;

        // Kiểm tra mép vực
        Vector2 cliffCheckPos = (Vector2)touchingCol.bounds.center + wallCheckDirection * (touchingCol.bounds.size.x * 0.5f + cliffDistance);
        IsNearCliff = touchingCol.Cast(wallCheckDirection, castFilter, cliffHits, cliffDistance) == 0 &&
                     touchingCol.Cast(Vector2.down, castFilter, cliffHits, cliffDistance * 2f) == 0 &&
                     !IsGrounded;
    }
}