using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject player; // Kéo Player vào từ Inspector

    public static bool IsGameOver = false;

    private bool isPaused = false;

    void Update()
    {
        if (IsGameOver) return;

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

        // 🟢 AUTO SAVE khi Pause
        Vector2 pos = player.transform.position;
        float health = player.GetComponent<Damageable>().GetCurrentHealth();

        SaveSystem.SaveGame(pos, health);
    }

    public void TryAgain()
    {
        IsGameOver = false;
        Time.timeScale = 1f;

        GameSessionManager.Instance?.IncrementRun();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        IsGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
