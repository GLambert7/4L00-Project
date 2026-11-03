using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = false;

    //made this one so that it doesnt spam pause and unpause
    private bool wasPausePressed = false;

    // Pause menu
    [SerializeField] GameObject pauseMenu;

    void Update()
    {
        if (PlayerInputHandler.Instance == null)
            return;

        //basically asking if the pause button is being held down
        bool pausePressed = PlayerInputHandler.Instance.PauseInput;

        // Detect when Tab is first pressed
        if (pausePressed && !wasPausePressed)
        {
            TogglePause();
        }

        wasPausePressed = pausePressed;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // Set menu as active based on pause bool
        pauseMenu.SetActive(isPaused);

        // Pause or resume the game
        Time.timeScale = isPaused ? 0f : 1f;

        // Cursor lock toggle
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Debug.Log("Game Paused: " + isPaused);
    }

    private void OnDisable()
    {
        // Reset time when disabled
        Time.timeScale = 1f;
        isPaused = false;
        wasPausePressed = false;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
