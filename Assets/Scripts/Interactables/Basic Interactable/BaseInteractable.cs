using Unity.VisualScripting;
using UnityEngine;

public class BaseInteractable : MonoBehaviour
{
    #region Variables
    // Who can interact with this object? These booleans can be set in the Unity Inspector for each interactable object.
    public bool interactableByDolphin = false; // Can the dolphin interact with this object?
    public bool interactableByOctopus = false; // Can the octopus interact with this object?
    public bool interactableByEel = false; // Can the eel interact with this object?
    public bool interactableByPlayer = false; // Can player interact with this object?

    // Basic requirements for each interactable object to function properly.
    [Header("Player Input Handler")]
    [SerializeField] protected PlayerInputHandler _inputHandler; // Reference to the PlayerInputHandler script, which handles player input. This should be set up in the Unity Editor.
    [SerializeField] protected Collider interactionArea; // The area in which the creature can interact with the object. This is a trigger collider that should be set up in the Unity Editor.
    [SerializeField] protected AudioSource cantUseSound; // The sound that plays when the creature tries to interact with the object but cannot. This should be set up in the Unity Editor.
    [SerializeField] protected AudioSource useSound; // The sound that plays when the creature interacts with the object. This should be set up in the Unity Editor.
    [SerializeField] protected GameObject PlayerRef; // Reference to our player object

    protected bool canInteract = false; // Is the creature in range to interact with the object? This is set to true when the creature enters the interaction area, and false when it exits.
    protected bool hasInteracted = false; // Has the creature already interacted with the object? This is set to true when the creature interacts with the object, and false when the interaction key for all creatures is in the released state.
    #endregion

    #region Unity Methods
    void Update()
    {
        InteractionCheck();
    }
    #endregion

    #region Interaction Methods
    public virtual void DolphinInteract()
    {
        if (interactableByDolphin)
        {
            // Implement dolphin interaction logic here
            Debug.Log("Dolphin interacted with " + gameObject.name);
            if (useSound != null) useSound.Play();
        }
        else
        {
            Debug.Log("Dolphin cannot interact with " + gameObject.name);
            // Extremely Loud Incorrect Buzzer Sound Effect
            if (cantUseSound != null) cantUseSound.Play();
            return;
        }
    }
    public virtual void OctopusInteract()
    {
        if (interactableByOctopus)
        {
            // Implement octopus interaction logic here
            Debug.Log("Octopus interacted with " + gameObject.name);
            if (useSound != null) useSound.Play();
        }
        else
        {
            Debug.Log("Octopus cannot interact with " + gameObject.name);
            // Extremely Loud Incorrect Buzzer Sound Effect
            if (cantUseSound != null) cantUseSound.Play();
            return;
        }
    }
    public virtual void EelInteract()
    {
        if (interactableByEel)
        {
            // Implement eel interaction logic here
            Debug.Log("Eel interacted with " + gameObject.name);
            if (useSound != null) useSound.Play();
        }
        else
        {
            Debug.Log("Eel cannot interact with " + gameObject.name);
            // Extremely Loud Incorrect Buzzer Sound Effect
            if (cantUseSound != null) cantUseSound.Play();
            return;
        }
    }
    public virtual void PlayerInteract()
    {
        if (interactableByPlayer)
        {
            // Implement player interaction logic here
            Debug.Log("Player interacted with " + gameObject.name);
            if (useSound != null) useSound.Play();
        }
        else
        {
            Debug.Log("Player cannot interact with " + gameObject.name);
            // Extremely Loud Incorrect Buzzer Sound Effect
            if (cantUseSound != null) cantUseSound.Play();
            return;
        }
    }

    // Check if the player is in range and has pressed the interact button, if so, set isMoving to true, starting the movement of the block
    protected void InteractionCheck()
    // Each creature has its own interact button, so we check for each one separately. If the player is in range and presses the interact button, we call the appropriate interaction method for that creature.
    {
        if (!canInteract) return;
        if (!interactableByPlayer && !interactableByDolphin && !interactableByOctopus && !interactableByEel) return;
        if (!_inputHandler.PlayerInteractInput && !_inputHandler.DolphinInteractInput && !_inputHandler.OctopusInteractInput && !_inputHandler.EelInteractInput)
        {
            hasInteracted = false;
        }

        if (hasInteracted) return;

        if (canInteract && _inputHandler.PlayerInteractInput)
        {
            hasInteracted = true;
            PlayerInteract();
        }
        else if (canInteract && _inputHandler.DolphinInteractInput)
        {
            hasInteracted = true;
            DolphinInteract();
        }
        else if (canInteract && _inputHandler.OctopusInteractInput)
        {
            hasInteracted = true;
            OctopusInteract();
        }
        else if (canInteract && _inputHandler.EelInteractInput)
        {
            hasInteracted = true;
            EelInteract();
        }
    }

    public virtual void Activate()
    {
        // Implement activation logic here
        // Debug.Log(gameObject.name + " has been activated.");
    }

    #endregion


    #region Trigger Events
    // Trigger events to check if the player is entering or exiting the range of the block, and setting canInteract to true or false accordingly
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
    #endregion
}
