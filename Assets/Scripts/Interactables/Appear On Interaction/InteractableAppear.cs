using UnityEngine;

public class InteractableAppear : MonoBehaviour
{
    // Attach this script not to the object you want to appear, but to the object that will trigger the appearance of the other object (Typically an empty object with a collider that defines the space in which the interaction will be allowed to occur.) The object that will appear should be assigned to the _interactableObject variable in the inspector. The object that this script is assigned to will trigger the appearance, and should have a collider with "Is Trigger" checked. Put this collider into the _interactionCollider field in the inspector. The player should be tagged as "Player" to ensure it is detected correctly.

    #region Variables
    [Header("Player Input Handler")]
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private GameObject _interactableObject;
    [SerializeField] private Collider _interactionCollider;
    

    private bool _playerInRange = false;
    #endregion

    #region Unity Methods
    private void Start()
    {
        _interactableObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInRange && _inputHandler.PlayerInteractInput)
        {
            // Perform the interaction logic here
            Debug.Log("Interacted with: " + gameObject.name);
            _interactableObject.SetActive(true);
        }
    }
    #endregion

    #region Trigger Logic and Methods
    // When the player enters the trigger collider, set _playerInRange to true. When the player exits the trigger collider, set _playerInRange to false. This variable controls if the player can interact with the object or not. If the player is in range and presses the interact button, the interactable object will appear.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }
    #endregion
}
