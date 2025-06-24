using UnityEngine;

public class ChaseState1 : EnemyBaseState
{
    private Transform player;

    public ChaseState1(WolfAI wolf) : base(wolf)
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public override void FixedUpdate()
    {
        if (wolf.LockVelocity || player == null) return;

        Vector2 direction = player.position.x > wolf.transform.position.x ? Vector2.right : Vector2.left;
        wolf.SetWalkDirection(direction);

        if (wolf.CanMove)
            wolf.MoveForward();
        else
            wolf.StopMovement();

        if (wolf.Detector.PlayerInAttackRange)
        {
            wolf.ChangeState(new AttackState1(wolf));
        }

        if (!wolf.Detector.PlayerInSight)
        {
            wolf.ChangeState(new PatrolState1(wolf));
        }
    }
}
