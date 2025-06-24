using UnityEngine;

public class AttackState1 : EnemyBaseState
{
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public AttackState1(WolfAI wolf) : base(wolf) { }

    public override void Enter()
    {
        wolf.StopMovement();
        wolf.TriggerAttack();
        lastAttackTime = Time.time;
    }

    public override void FixedUpdate()
    {
        if (!wolf.Detector.PlayerInAttackRange)
        {
            wolf.ChangeState(new ChaseState1(wolf));
            return;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            wolf.TriggerAttack();
            lastAttackTime = Time.time;
        }
    }
}
