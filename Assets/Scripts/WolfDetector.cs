using UnityEngine;

public class WolfDetector : MonoBehaviour
{
    public bool PlayerInSight { get; set; }

    public bool PlayerInAttackRange
    {
        get
        {
            bool inRange = Time.time < lastDetectedTime + graceDuration;
            Debug.Log($"[WolfDetector] Attack check: {inRange} (now: {Time.time:F2}, until: {lastDetectedTime + graceDuration:F2})");
            return inRange;
        }
    }

    [SerializeField] private float graceDuration = 0.5f;
    private float lastDetectedTime = Mathf.NegativeInfinity;

    public void SetAttackRangeDetected()
    {
        lastDetectedTime = Time.time;
        Debug.Log("🔴 Player in attack range (timestamp set)");
    }
}
