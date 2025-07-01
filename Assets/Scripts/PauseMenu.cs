using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuUI; // Kéo Panel "PauseMenu" vào đây

    private bool isPaused = false;
    public static bool IsGameOver = false;


    void Update()
    {
        if (IsGameOver) return; // ⚠️ chặn khi đang GameOver

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void TryAgain()
    {
        PauseMenu.IsGameOver = false;
        Time.timeScale = 1f;

        // ✅ Tính lại Run
        GameSessionManager.Instance?.IncrementRun();

        // Load lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Đổi thành tên scene menu của bạn
    }
}
