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
        Debug.Log($"[DetectionZone {name}] TriggerEnter with: {other.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log($"[DetectionZone {name}] Detected player with type: {zoneType}");

            if (zoneType == ZoneType.Sight)
                detector.PlayerInSight = true;

            if (zoneType == ZoneType.Attack)
                detector.PlayerInAttackRange = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (zoneType == ZoneType.Sight)
                detector.PlayerInSight = false;
            if (zoneType == ZoneType.Attack)
                detector.PlayerInAttackRange = false;
        }
    }
}
