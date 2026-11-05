using UnityEngine;
using UnityEngine.UI;

public class PopupIndicator : MonoBehaviour
{
    public Transform mainCam;
    public Transform target;
    public Transform worldSpaceCanvas;

    // Icon image
    private Image indicator;

    // Reference parent interactable to check if player is in range
    public BaseInteractable interactable;

    // Offset the indicator above the object adn edit in inspector
    [SerializeField] private Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get variables from parent
        target = transform.parent;
        interactable = GetComponentInParent<BaseInteractable>();
        indicator = GetComponent<Image>();

        // Get variables in scene
        mainCam = Camera.main.transform;
        worldSpaceCanvas = FindFirstObjectByType<Canvas>().transform;
        transform.SetParent(worldSpaceCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        transform.position = target.position + offset;

        if (interactable.canInteract)
        {
            if (!interactable.hasInteracted)
            {
                indicator.enabled = true;
            }            
        }
        else
        {
            indicator.enabled = false;
        }
    }
}
