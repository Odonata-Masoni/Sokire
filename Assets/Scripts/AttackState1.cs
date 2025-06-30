using UnityEngine;
using System.Collections;

public class AttackState1 : EnemyBaseState
{
    private float attackCooldown = 0.75f;
    private float lastAttackTime;
    private bool isAttacking;

    private bool waitAfterAttack = false;
    private float waitTimer = 0f;
    private float postAttackBuffer = 0.15f; // Delay ngắn sau khi đánh xong

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
        Debug.Log($"[AttackState1] Update - isAttacking: {isAttacking}, waitAfterAttack: {waitAfterAttack}, InSight: {wolf.Detector.PlayerInSight}, InAttack: {wolf.Detector.PlayerInAttackRange}");

        // Nếu đang tấn công thì không xử lý gì thêm
        if (isAttacking) return;

        // Nếu vừa đánh xong thì chờ một chút trước khi xét đổi state
        if (waitAfterAttack)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                waitAfterAttack = false;
            }
            else
            {
                return;
            }
        }

        // Nếu không còn thấy player
        if (!wolf.Detector.PlayerInSight && !wolf.Detector.PlayerInAttackRange)
        {
            Debug.Log("[AttackState1] Player lost - switching to Patrol");
            wolf.ChangeState(new PatrolState1(wolf));
            return;
        }

        // Nếu thấy player nhưng không trong tầm đánh
        if (!wolf.Detector.PlayerInAttackRange && wolf.Detector.PlayerInSight)
        {
            Debug.Log("[AttackState1] Player out of range - switching to Chase");
            wolf.ChangeState(new ChaseState1(wolf));
            return;
        }

        // Nếu đủ cooldown thì tấn công tiếp
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("[AttackState1] Cooldown passed - starting next attack");
            StartAttack();
        }
    }

    public override void FixedUpdate()
    {
        wolf.StopMovement(); // Đảm bảo đứng yên khi tấn công
    }

    public override void Exit()
    {
        Debug.Log("[AttackState1] Exit Attack");
        wolf.SetCanMove(true);
        isAttacking = false;
        waitAfterAttack = false;
    }

    private void StartAttack()
    {
        Debug.Log("[AttackState1] StartAttack()");
        wolf.TriggerAttack();
        lastAttackTime = Time.time;
        isAttacking = true;
    }

    // Gọi từ animation event
    public void OnAttackAnimationComplete()
    {
        wolf.StartCoroutine(HandleAttackEndDelayed());
    }

    private IEnumerator HandleAttackEndDelayed()
    {
        yield return null; // Đợi 1 frame để các collider được cập nhật

        Debug.Log("[AttackState1] Attack animation complete (delayed)");
        isAttacking = false;

        // Nếu player còn trong vùng attack → giữ state Attack, chờ cooldown
        if (wolf.Detector.PlayerInAttackRange)
        {
            Debug.Log("[AttackState1] Player vẫn trong attack range - giữ trạng thái Attack");
            waitAfterAttack = true;
            waitTimer = postAttackBuffer;
            yield break;
        }

        // Nếu không còn trong vùng đánh
        if (wolf.Detector.PlayerInSight)
        {
            Debug.Log("[AttackState1] Player còn trong Sight - chuyển sang Chase");
            wolf.ChangeState(new ChaseState1(wolf));
        }
        else
        {
            Debug.Log("[AttackState1] Player biến mất - chuyển sang Patrol");
            wolf.ChangeState(new PatrolState1(wolf));
        }
    }
}
