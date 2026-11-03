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
}
