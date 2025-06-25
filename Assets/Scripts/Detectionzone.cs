using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public enum ZoneType { Sight, Attack }
    public ZoneType zoneType;

    private WolfDetector detector;

    private void Start()
    {
        detector = GetComponentInParent<WolfDetector>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[DetectionZone {name}] TriggerEnter with: {other.name}");

        if (zoneType == ZoneType.Sight)
        {
            detector.PlayerInSight = true;
            Debug.Log("🟢 Player detected");
        }

        if (zoneType == ZoneType.Attack)
        {
            detector.SetAttackRangeDetected(); // ✅ sử dụng grace time
        }

        Debug.Log($"🧪 AttackZone Size: {GetComponent<BoxCollider2D>().size}, Center: {GetComponent<BoxCollider2D>().offset}");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (zoneType == ZoneType.Sight)
        {
            detector.PlayerInSight = false;
            Debug.Log("⚪ Player lost");
        }

        // ❌ KHÔNG được đụng tới PlayerInAttackRange nữa
        // vì nó được tính bằng grace time trong WolfDetector
    }
}
