using UnityEngine;

public class AttackState1 : EnemyBaseState
{
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public AttackState1(WolfAI wolf) : base(wolf) { }

    public override void Enter()
    {
        Debug.Log("[AttackState1] Entered Attack");

        wolf.StopMovement();
        wolf.SetCanMove(false);
        wolf.TriggerAttack();
        lastAttackTime = Time.time;

    }


    public override void Update()
    {
        if (!wolf.Detector.PlayerInAttackRange && !wolf.Detector.PlayerInSight)
        {
            Debug.Log("[AttackState1] Player gone, switching to Patrol");
            wolf.ChangeState(new PatrolState1(wolf));
            return;
        }

        if (!wolf.Detector.PlayerInAttackRange && wolf.Detector.PlayerInSight)
        {
            Debug.Log("[AttackState1] Player not in attack range, chasing again");
            wolf.ChangeState(new ChaseState1(wolf));
            return;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("[AttackState1] Triggering attack");
            wolf.TriggerAttack();
            lastAttackTime = Time.time;
        }
    }

    public override void Exit()
    {
        Debug.Log("[AttackState1] Exit Attack");
        wolf.SetCanMove(true);
    }
}
