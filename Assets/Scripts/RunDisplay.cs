using UnityEngine;
using TMPro;

public class RunDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text runText;

    private void Start()
    {
        if (runText != null && GameSessionManager.Instance != null)
        {
            runText.text = "RUN#: " + GameSessionManager.Instance.RunCount;
        }
    }
    public void ResetRun()
    {
        PlayerPrefs.DeleteKey("RunCount");
    }

}
