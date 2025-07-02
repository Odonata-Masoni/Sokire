using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private ExecutionerAI executioner;

    private void Start()
    {
        executioner = GetComponentInParent<ExecutionerAI>();
        if (executioner == null)
            Debug.LogWarning("⚠️ Không tìm thấy ExecutionerAI trong parent!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            executioner.PlayerInAttackZone = true;
            Debug.Log("🟢 Player vào vùng AttackZone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            executioner.PlayerInAttackZone = false;
            Debug.Log("🔴 Player rời vùng AttackZone");
        }
    }
}
