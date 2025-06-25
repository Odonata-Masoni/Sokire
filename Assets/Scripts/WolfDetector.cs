using UnityEngine;

public class WolfDetector : MonoBehaviour
{
    public bool PlayerInSight { get; set; }

    public bool PlayerInAttackRange
    {
        get { return Time.time < lastDetectedTime + graceDuration; }
    }

    [SerializeField] private float graceDuration = 0.5f;
    private float lastDetectedTime = Mathf.NegativeInfinity;

    public void SetAttackRangeDetected()
    {
        lastDetectedTime = Time.time;
        Debug.Log("🔴 Player in attack range (with grace)");
    }
}
