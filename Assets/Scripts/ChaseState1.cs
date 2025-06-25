using UnityEngine;

public class ChaseState1 : EnemyBaseState
{
    private Transform player;
    private float entryTime;

    public ChaseState1(WolfAI wolf) : base(wolf)
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public override void Enter()
    {
        Debug.Log("[ChaseState1] Entered Chase");
        entryTime = Time.time;
    }

    public override void FixedUpdate()
    {
        Debug.Log($"[ChaseState1] FixedUpdate - Sight: {wolf.Detector.PlayerInSight}, Attack: {wolf.Detector.PlayerInAttackRange}");

        if (!wolf.Detector.PlayerInSight && !wolf.Detector.PlayerInAttackRange)
        {
            Debug.Log("[ChaseState1] ❌ Lost sight of player, switching to Patrol");
            wolf.ChangeState(new PatrolState1(wolf));
            return;
        }

        if (wolf.Detector.PlayerInAttackRange)
        {
            Debug.Log("[ChaseState1] ✅ Player in attack range, switching to Attack");
            wolf.ChangeState(new AttackState1(wolf));
            return;
        }

        if (wolf.LockVelocity)
        {
            Debug.Log("[ChaseState1] ⛔ LockVelocity == true, skipping movement");
            return;
        }

        Vector2 dir = player.position.x > wolf.transform.position.x ? Vector2.right : Vector2.left;
        wolf.SetWalkDirection(dir);

        if (wolf.CanMove)
        {
            Debug.Log("[ChaseState1] ✔ Moving forward");
            wolf.MoveForward();
        }
        else
        {
            Debug.Log("[ChaseState1] ⛔ Can't move");
            wolf.StopMovement();
        }
    }
}
