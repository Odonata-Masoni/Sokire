using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;

    private Animator animator;
    private CollisionChecker collisionChecker;

    void Awake()
    {
        animator = GetComponent<Animator>();
        collisionChecker = GetComponent<CollisionChecker>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (Time.time < lastAttackTime + attackCooldown) return;

        if (!collisionChecker.IsGrounded && !collisionChecker.IsTouchingWall) return;

        lastAttackTime = Time.time;
        animator.SetTrigger(AnimationStrings.isAttacking);
    }
}
