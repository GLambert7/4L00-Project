using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class OptionsMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }
}
