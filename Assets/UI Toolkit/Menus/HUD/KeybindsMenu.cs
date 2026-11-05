using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class KeybindsMenu : MonoBehaviour
{
    // Elements
    private VisualElement _root;

    // References
    private InputActionAsset inputActions;

    private void OnEnable()
    {
        // Query root visual elements for each menu screen
        _root = GetComponent<UIDocument>().rootVisualElement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
