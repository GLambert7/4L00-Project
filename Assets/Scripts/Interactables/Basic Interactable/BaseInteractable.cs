using System;
using Unity.VisualScripting;
using UnityEngine;

public class BaseInteractable : MonoBehaviour
{

    #region Variables
    // Who can interact with this object? These booleans can be set in the Unity Inspector for each interactable object.
    [Header("Interactable Booleans")]
    [Tooltip("Can the dolphin interact with this object?")] 
    public bool interactableByDolphin = false; // Can the dolphin interact with this object?
    [Tooltip("Can the octopus interact with this object?")] 
    public bool interactableByOctopus = false; // Can the octopus interact with this object?
    [Tooltip("Can the eel interact with this object?")] 
    public bool interactableByEel = false; // Can the eel interact with this object?
    [Tooltip("Can the player interact with this object independently from the companions?")] 
    public bool interactableByPlayer = false; // Can player interact with this object?

    // Basic requirements for each interactable object to function properly.
    [Header("Object References")]
    [Tooltip("This should be filled with the object titled \"InputHandler\", which should be placed in the scene already. If this variable is empty, click the target icon in this variable and it should pull up the correct object.")]
    [SerializeField] protected PlayerInputHandler _inputHandler; // Reference to the PlayerInputHandler script, which handles player input. This should be set up in the Unity Editor.
    [Tooltip("This should be filled with a sphere collider placed on the object, which is <b>SEPERATE FROM THE MAIN COLLIDER and SET TO BE A TRIGGER</b>, as this is what detects if the player is near the object and allows for interaction.")]
    [SerializeField] protected Collider interactionArea; // The area in which the creature can interact with the object. This is a trigger collider that should be set up in the Unity Editor.
    [Tooltip("This should be filled with a reference to the Player object. If this is empty, find the object in the scene named \"Player\" or \"Finley\", then drag and drop it from the hierarchy to here.")]
    [SerializeField] protected GameObject PlayerRef; // Reference to our player object
    
    [Header("Sound References")]
    [Tooltip("This should be filled with the appropriate audio file for when the player attempts to use the object in the wrong way, such as interacting with a Dolphin related object with the Eel.")]
    [SerializeField] protected AudioSource cantUseSound; // The sound that plays when the creature tries to interact with the object but cannot. This should be set up in the Unity Editor.
    [Tooltip("This should be filled with the appropriate sound for when <b>The Player</b> successfully interacts with an object. <b>If your sound is not playing when you interact with an object, make sure to check if it is assigned to the correct variable!</b>")]
    [SerializeField] protected AudioSource useSound; // The sound that plays when the creature interacts with the object. This should be set up in the Unity Editor.
    [Tooltip("This should be filled with the appropriate sound for when <b>The Dolphin</b> successfully interacts with an object. <b>If your sound is not playing when you interact with an object, make sure to check if it is assigned to the correct variable!</b>")]
    [SerializeField] protected AudioSource useSoundDolphin; // The sound that plays when the dolphin interacts with the object. This should be set up in the Unity Editor.
    [Tooltip("This should be filled with the appropriate sound for when <b>The Octopus</b> successfully interacts with an object. <b>If your sound is not playing when you interact with an object, make sure to check if it is assigned to the correct variable!</b>")]
    [SerializeField] protected AudioSource useSoundOctopus; // The sound that plays when the octopus interacts with the object. This should be set up in the Unity Editor.
    [Tooltip("This should be filled with the appropriate sound for when <b>The Eel</b> successfully interacts with an object. <b>If your sound is not playing when you interact with an object, make sure to check if it is assigned to the correct variable!</b>")]
    [SerializeField] protected AudioSource useSoundEel; // The sound that plays when the eel interacts with the object. This should be set up in the Unity Editor.


    [HideInInspector] public bool canInteract = false; // Is the creature in range to interact with the object? This is set to true when the creature enters the interaction area, and false when it exits.
    [HideInInspector] public bool hasInteracted = false; // Has the creature already interacted with the object? This is set to true when the creature interacts with the object, and false when the interaction key for all creatures is in the released state.

    //stores the objects that creatures are assigned to 
    private static BaseInteractable dolphinStandby;
    private static BaseInteractable octopusStandby;
    private static BaseInteractable eelStandby;

    // Ignore the things going on below here I just want to put a line of text notating where the BaseInteractable variables end so I can seperate them from the inheritor class variables
    [Serializable] private struct EmptySpace { }

    [Header("---End of BaseInteractable Variables---")]
    [SerializeField] private EmptySpace _footer;



    #endregion

    #region Unity Methods
    void Update()
    {
        if (_inputHandler == null) { _inputHandler = FindObjectOfType<PlayerInputHandler>(); }
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
            if (useSoundDolphin != null) useSoundDolphin.Play();
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
            if (useSoundOctopus != null) useSoundOctopus.Play();
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
            if (useSoundEel != null) useSoundEel.Play();
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
{
    if (_inputHandler == null)
    {
        _inputHandler = PlayerInputHandler.Instance;
        if (_inputHandler == null) return;
    }
    //checks if the player is canceling one of the companions
    CancelStandby();

    if (!_inputHandler.PlayerInteractInput)
        hasInteracted = false;

    if (canInteract && _inputHandler.PlayerInteractInput && !hasInteracted)
    {
        hasInteracted = true;
        PlayerInteract();
    }

    // Missy
    if (_inputHandler.DolphinJustPressed)
    {
        //if Missy is already assigned to an object, activate her
        if (dolphinStandby == this)
        {
            dolphinStandby = null;
            DolphinInteract();
        }
        //if she isnt assigned to an object assign her
        else if (dolphinStandby == null && canInteract && interactableByDolphin)
        {
            dolphinStandby = this;
            //saves blocks position
            if (this is PushBlock pushBlock)
            {
                pushBlock.SavePushPosition();
            }
            Debug.Log("Dolphin assigned to " + gameObject.name);
        }
    }

    // Dumbo
    if (_inputHandler.OctopusJustPressed)
    {
        //activate if dumbo si assigned 
        if (octopusStandby == this)
        {
            octopusStandby = null;
            OctopusInteract();
        }
        //if not assigned activcate
        else if (octopusStandby == null && canInteract && interactableByOctopus)
        {
            octopusStandby = this;
            Debug.Log("Octopus assigned to " + gameObject.name);
        }
    }

    // Sparky (Shaqeel O'Neel)
    if (_inputHandler.EelJustPressed)
    {
        //activatse if SParky is assigned 
        if (eelStandby == this)
        {
            eelStandby = null;
            EelInteract();
        }
        //if sparky not assigned assign him
        else if (eelStandby == null && canInteract && interactableByEel)
        {
            eelStandby = this;
            Debug.Log("Eel assigned to " + gameObject.name);
        }
    }
}


private void CancelStandby()
{
    // Only check when the cancel button is pressed
    if (!_inputHandler.CancelJustPressed) return;

    // Find the player's camera
    Camera playerCamera = Camera.main;
    if (playerCamera == null) return;

   //raycast
    Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

    if (Physics.Raycast(ray, out RaycastHit hit, 100f))
    {
        // What player is looking at
        BaseInteractable target = hit.collider.GetComponentInParent<BaseInteractable>();

        // Only cancel if pklayer is looking at the object
        if (target != this) return;

        //clears Missy standby
        if (dolphinStandby == this)
        {
            dolphinStandby = null;
            Debug.Log("Dolphin standby cancelled");
        }
        //clears Dumbo standy
        if (octopusStandby == this)
        {
            octopusStandby = null;
            Debug.Log("Octopus standby cancelled");
        }

        //clears Sparky standby
        if (eelStandby == this)
        {
            eelStandby = null;
            Debug.Log("Eel standby cancelled");
        }
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

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
    #endregion
}
