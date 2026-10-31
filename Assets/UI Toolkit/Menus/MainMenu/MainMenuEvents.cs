using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    #region Variables and Awake
    // Documents
    private VisualElement _root;
    private VisualElement _titleMenuRoot;
    private VisualElement _optionsRoot;
    private VisualElement _creditsRoot;

    // *** UI VisualElement Variables ***
    // Title
    private Button startButton;
    private Button optionsButton;
    private Button creditsButton;
    private Button quitButton;

    // Options and Credits
    private Button returnButton;

    private void Awake()
    {
        // Query root visual elements for each menu screen
        _root = GetComponent<UIDocument>().rootVisualElement;
        _titleMenuRoot = _root.Q<VisualElement>("TitleMenu");
        _optionsRoot = _root.Q<VisualElement>("OptionsMenu");
        _creditsRoot = _root.Q<VisualElement>("CreditsMenu");

        // *** Assign Buttons ***
        //Title Menu
        startButton = _titleMenuRoot.Q("StartButton") as Button;
        optionsButton = _titleMenuRoot.Q("OptionsButton") as Button;
        creditsButton = _titleMenuRoot.Q("CreditsButton") as Button;
        quitButton = _titleMenuRoot.Q("QuitButton") as Button;

        // Options and Credits
        returnButton = _root.Q("ReturnButton") as Button;

        // *** Register Click Events ***
        // Title Menu
        startButton.clicked += () => OnStartClick();
        optionsButton.clicked += () => MenuEnter(_optionsRoot);
        creditsButton.clicked += () => MenuEnter(_creditsRoot);
        quitButton.clicked += () => OnQuitClick();

        // Options Menu
        returnButton.clicked += () => MenuEnter(_titleMenuRoot);

        // Credits Menu

        // Default menu visibility
        _optionsRoot.style.display = DisplayStyle.None;
        _creditsRoot.style.display = DisplayStyle.None;
    }
    #endregion

    // NOTE: This method is kinda important for univeral UI So I will maybe move it and make it public at some point -Cadence
    private void MenuEnter(VisualElement enter)
    {
        // Turn off all menu displays
        _titleMenuRoot.style.display = DisplayStyle.None;
        _optionsRoot.style.display = DisplayStyle.None;
        _creditsRoot.style.display = DisplayStyle.None;

        // Turn on menu user is entering
        enter.style.display = DisplayStyle.Flex;
    }

    #region Title Menu Button Events
    private void OnStartClick()
    {
        Debug.Log("Pressed Start");

        // Switch scenes to main game
        SceneManager.LoadScene(1);
    }

    private void OnQuitClick()
    {
        Debug.Log("Pressed Quit");

        // Quits game
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    #endregion
}
