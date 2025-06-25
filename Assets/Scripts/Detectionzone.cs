using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public enum ZoneType { Sight, Attack }
    public ZoneType zoneType;

    private WolfDetector detector;

    private void Start()
    {
        // Lấy đúng WolfDetector từ object cha
        detector = GetComponentInParent<WolfDetector>();
        if (detector == null)
        {
            Debug.LogError($"[DetectionZone {name}] ❌ Không tìm thấy WolfDetector ở cha!");
        }
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
            detector.SetAttackRangeDetected();
        }

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            Debug.Log($"🧪 {name} Size: {box.size}, Center: {box.offset}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (zoneType == ZoneType.Sight)
        {
            detector.PlayerInSight = false;
            Debug.Log("⚪ Player lost");
        }
    }
}
