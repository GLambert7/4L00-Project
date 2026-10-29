using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private Button startButton;
    private Button optionsButton;
    private Button creditsButton;
    private Button quitButton;

    // Assigning needed elements on Awake
    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        // Button definitions
        startButton = _document.rootVisualElement.Q("StartButton") as Button;
        optionsButton = _document.rootVisualElement.Q("OptionsButton") as Button;
        creditsButton = _document.rootVisualElement.Q("CreditsButton") as Button;
        quitButton = _document.rootVisualElement.Q("QuitButton") as Button;

        // Register click events
        startButton.RegisterCallback<ClickEvent>(OnStartClick);
        optionsButton.RegisterCallback<ClickEvent>(OnOptionsClick);
        creditsButton.RegisterCallback<ClickEvent>(OnCreditsClick);
        quitButton.RegisterCallback<ClickEvent>(OnQuitClick);

    }

    // On Start click
    private void OnStartClick(ClickEvent evt)
    {
        Debug.Log("Pressed Start");

        // Switch scenes to main game
        SceneManager.LoadScene(1);
    }

    // On Options click
    private void OnOptionsClick(ClickEvent evt)
    {
        Debug.Log("Pressed Options");
    }

    // On Credits click
    private void OnCreditsClick(ClickEvent evt)
    {
        Debug.Log("Pressed Credits");
    }

    // On Quit click
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
}
