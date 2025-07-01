using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Dừng game
    }

    public void Replay()
    {
        Time.timeScale = 1f;

        // ✅ Tính lại số run
        GameSessionManager.Instance.IncrementRun();

        // Load lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game called (chỉ thấy trong Build, không thấy trong Editor)");
    }
}
