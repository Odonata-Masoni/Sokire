using UnityEngine;

public class WolfDetector : MonoBehaviour
{
    public bool PlayerInSight { get;  set; }
    public bool PlayerInAttackRange { get;  set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (name.Contains("PlayerDetection"))
            {
                PlayerInSight = true;
                Debug.Log("🟢 Player detected");
            }

            if (name.Contains("AttackZone"))
            {
                PlayerInAttackRange = true;
                Debug.Log("🔴 Player in attack range");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (name.Contains("PlayerDetection"))
            {
                PlayerInSight = false;
                Debug.Log("⚪ Player lost");
            }

            if (name.Contains("AttackZone"))
            {
                PlayerInAttackRange = false;
                Debug.Log("⚫ Player left attack range");
            }
        }
    }
}
