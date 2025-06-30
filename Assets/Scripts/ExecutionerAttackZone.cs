using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public ExecutionerAI executioner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            executioner.PlayerInAttackZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            executioner.PlayerInAttackZone = false;
        }
    }
}
