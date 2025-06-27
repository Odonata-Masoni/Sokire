using UnityEngine;

public class AttackState1 : EnemyBaseState
{
    private float attackCooldown = 1.5f;
    private float lastAttackTime;
    private bool isAttacking;

    public AttackState1(WolfAI wolf) : base(wolf) { }

    public override void Enter()
    {
        Debug.Log("[AttackState1] Entered Attack");

        wolf.StopMovement();
        wolf.SetCanMove(false);
        StartAttack();
    }

    public override void Update()
    {
        // Trong lúc đang attack thì không xét chuyển trạng thái
        if (isAttacking) return;

        // Nếu không còn thấy player → quay về patrol
        if (!wolf.Detector.PlayerInSight && !wolf.Detector.PlayerInAttackRange)
        {
            //Debug.Log("[AttackState1] Player gone, switching to Patrol");
            wolf.ChangeState(new PatrolState1(wolf));
            return;
        }

        // Nếu vẫn thấy player nhưng ngoài tầm đánh → đuổi tiếp
        if (!wolf.Detector.PlayerInAttackRange && wolf.Detector.PlayerInSight)
        {
            //Debug.Log("[AttackState1] Player not in attack range, chasing again");
            wolf.ChangeState(new ChaseState1(wolf));
            return;
        }

        // Nếu player còn trong vùng đánh → đánh tiếp sau cooldown
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            StartAttack();
        }
    }

    public override void FixedUpdate()
    {
        wolf.StopMovement(); // Luôn giữ đứng yên khi attack
    }

    public override void Exit()
    {
        Debug.Log("[AttackState1] Exit Attack");
        wolf.SetCanMove(true);
        isAttacking = false;
    }

    private void StartAttack()
    {
        //Debug.Log("[AttackState1] Triggering attack");
        wolf.TriggerAttack();
        lastAttackTime = Time.time;
        isAttacking = true;
    }

    // Gọi từ animation event
    public void OnAttackAnimationComplete()
    {
        Debug.Log("[AttackState1] OnAttackAnimationComplete()");
        isAttacking = false;

        // Nếu player vẫn còn trong vùng đánh, giữ lại state Attack
        if (wolf.Detector.PlayerInAttackRange)
        {
            // giữ lại
            Debug.Log("[AttackState1] Still in attack range – waiting for cooldown");
        }
        else if (wolf.Detector.PlayerInSight)
        {
            wolf.ChangeState(new ChaseState1(wolf));
        }
        else
        {
            wolf.ChangeState(new PatrolState1(wolf));
        }
    }

}
