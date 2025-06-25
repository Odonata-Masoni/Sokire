using UnityEngine;

public class PatrolState1 : EnemyBaseState
{
    public PatrolState1(WolfAI wolf) : base(wolf) { }

    public override void Enter()
    {
        wolf.SetWalkDirection(Vector2.left);
        Debug.Log("Entered [Patrol]"); // đặt theo từng state cụ thể
    }

    public override void FixedUpdate()
    {
        if (wolf.LockVelocity) return;

        if (wolf.CanMove)
            wolf.MoveForward();
        else
            wolf.StopMovement();

        // Đổi hướng nếu gặp tường hoặc gần vực
        if ((wolf.Touching.IsTouchingWall || wolf.Touching.IsNearCliff) && wolf.Touching.IsGrounded)
        {
            wolf.WallTouchTimer += Time.fixedDeltaTime;
            if (wolf.WallTouchTimer >= wolf.wallStuckTime)
            {
                FlipDirection();
                wolf.WallTouchTimer = 0f;
            }
        }
        else
        {
            wolf.WallTouchTimer = 0f;
        }

        // Phát hiện người chơi → chuyển sang Chase
        if (wolf.Detector.PlayerInSight)
        {
            wolf.ChangeState(new ChaseState1(wolf));
        }
    }

    private void FlipDirection()
    {
        Vector2 dir = wolf.WalkDirectionVector;
        wolf.SetWalkDirection(dir == Vector2.right ? Vector2.left : Vector2.right);
    }
  
}
