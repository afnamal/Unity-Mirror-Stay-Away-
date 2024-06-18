using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu; // Reference to the pause menu UI

    // Pauses the game
    public void Pause()
    {
        pauseMenu.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
    }

    // Loads the Main Menu scene
    public void Home()
    {
        Time.timeScale = 1f; // Ensure the game's time scale is reset
        StopAllMusic();      // Tüm müzikleri durdur
        SceneManager.LoadScene("MainMenu");
    }
    void StopAllMusic()
{
    // Tüm AudioSource componentlerini bul ve durdur
    AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
    foreach (AudioSource audio in audioSources)
    {
        audio.Stop();
    }
}
    // Resumes the game
    public void Resume()
    {
        pauseMenu.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game by setting time scale to 1
    }

    // Restarts the current level
    public void Restart()
    {
        Time.timeScale = 1f; // Ensure the game's time scale is reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    // Optionally, if you want to add a quit function
    public void QuitGame()
    {
        Time.timeScale = 1f; // Ensure the game's time scale is reset
        Application.Quit(); // Quit the application
    }

    void Update()
    {
        // Optionally handle pause/resume with a key (e.g., the Escape key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1f)
                Pause();
            else
                Resume();
        }
    }
}
