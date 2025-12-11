using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas; // Assign PauseMenuCanvas
    public Slider volumeSlider;    // Assign VolumeSlider
    private bool isPaused = false;

    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = 1f;       // Start on far right = max volume
            AudioListener.volume = 1f;     // Max volume
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Ensure pause menu is hidden at start
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
    }

    void Update()
    {
        // Toggle pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    // -------------------------
    // Volume Control (Right = Loud)
    // -------------------------
    public void SetVolume(float sliderValue)
    {
        // Direct mapping: left = quiet (0), right = loud (1)
        AudioListener.volume = sliderValue;
    }
}
