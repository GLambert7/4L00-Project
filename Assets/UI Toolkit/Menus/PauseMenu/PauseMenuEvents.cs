using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenuEvents : MonoBehaviour
{
    #region Variables and Awake
    // Elements
    private VisualElement _root;

    public bool isPaused;

    private Button resumeButton;
    private Button optionsButton;
    private Button mainMenuButton;
    private Button quitButton;

    // Assigning needed elements on Awake
    private void Awake()
    {
        // Query root visual elements for each menu screen
        _root = GetComponent<UIDocument>().rootVisualElement;

        // Button definitions
        resumeButton = _root.Q("ResumeButton") as Button;
        //optionsButton = _document.rootVisualElement.Q("OptionsButton") as Button;
        //mainMenuButton = _document.rootVisualElement.Q("MainMenuButton") as Button;
        quitButton = _root.Q("QuitButton") as Button;

        // Register click events
        //resumeButton.RegisterCallback<ClickEvent>(OnResumeClick);
        //optionsButton.RegisterCallback<ClickEvent>(OnOptionsClick);
        //mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenuClick);
        //quitButton.RegisterCallback<ClickEvent>(OnQuitClick);

        // Default visibility
        _root.style.display = DisplayStyle.None;
    }
    #endregion

    void Update()
    {
        /*
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
        */
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
        _root.style.display = DisplayStyle.Flex;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        _root.style.display = DisplayStyle.None;
    }
}
