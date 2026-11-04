using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenuEvents : MonoBehaviour
{
    #region Variables and Awake
    // Elements
    private VisualElement _root;
    private VisualElement _pauseMenuRoot;
    private VisualElement _optionsRoot;

    // Reference PauseManager
    [SerializeField] PauseManager _manager;

    private Button resumeButton;
    private Button optionsButton;
    private Button mainMenuButton;
    private Button quitButton;

    //Options Menu
    private Button returnButton;

    // Assigning needed elements OnEnable
    private void OnEnable()
    {
        // Query root visual elements for each menu screen
        _root = GetComponent<UIDocument>().rootVisualElement;
        _pauseMenuRoot = _root.Q<VisualElement>("PauseMenu");
        _optionsRoot = _root.Q<VisualElement>("OptionsMenu");

        // *** Assign Buttons ***
        // Pause Menu
        resumeButton = _root.Q("ResumeButton") as Button;
        optionsButton = _root.Q("OptionsButton") as Button;
        mainMenuButton = _root.Q("MainMenuButton") as Button;
        quitButton = _root.Q("QuitButton") as Button;

        //Options menu
        returnButton = _optionsRoot.Q("ReturnButton") as Button;

        // *** Register Click Events ***
        // Pause Menu
        resumeButton.clicked += () => _manager.TogglePause();
        optionsButton.clicked += () => MenuEnter(_optionsRoot);
        mainMenuButton.clicked += () => SceneManager.LoadScene(0);
        quitButton.clicked += () => OnQuitClick();


        //Options menu
        returnButton.clicked += () => MenuEnter(_pauseMenuRoot);
    }
    #endregion

    #region Pause Menu Button Events
    private void MenuEnter(VisualElement enter)
    {
        // Turn off all menu displays
        _pauseMenuRoot.style.display = DisplayStyle.None;
        _optionsRoot.style.display = DisplayStyle.None;

        // Turn on menu user is entering
        enter.style.display = DisplayStyle.Flex;
    }

    private void OnQuitClick()
    {
        // Quits game
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    #endregion
}
