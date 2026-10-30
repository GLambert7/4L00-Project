using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenuEvents : MonoBehaviour
{
    public bool isPaused;
    
    private UIDocument _document;

    private Button resumeButton;
    private Button optionsButton;
    private Button mainMenuButton;
    private Button quitButton;

    // Assigning needed elements on Awake
    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        // Button definitions
        resumeButton = _document.rootVisualElement.Q("ResumeButton") as Button;
        optionsButton = _document.rootVisualElement.Q("OptionsButton") as Button;
        mainMenuButton = _document.rootVisualElement.Q("MainMenuButton") as Button;
        quitButton = _document.rootVisualElement.Q("QuitButton") as Button;

        // Register click events
        resumeButton.RegisterCallback<ClickEvent>(OnResumeClick);
        //optionsButton.RegisterCallback<ClickEvent>(OnOptionsClick);
        //mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenuClick);
        quitButton.RegisterCallback<ClickEvent>(OnQuitClick);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // On Start click
    private void OnResumeClick(ClickEvent evt)
    {
        Debug.Log("Pressed Resume");

        // Switch scenes to main game
        SceneManager.LoadScene(1);
    }

    private void OnQuitClick(ClickEvent evt)
    {
        Debug.Log("Pressed Quit");

        // Quits game
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }

    // Pause and resume game
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
}
