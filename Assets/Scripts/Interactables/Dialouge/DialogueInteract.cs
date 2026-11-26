using UnityEngine;
using UnityEngine.UIElements;

public class DialogueInteract : BaseInteractable
{
    [Header("Dialogue Settings")]
    [SerializeField] private GameObject dialogueBox;
    private DialogueBox dialogueUI;

    private bool dialogueOpen = false;

    private void Awake()
    {
        dialogueUI = dialogueBox.GetComponent<DialogueBox>();
    }

    public override void PlayerInteract()
    {
        base.PlayerInteract();
        if (dialogueBox == null) { return; }

        if (dialogueOpen)
        {
            if (_inputHandler.PlayerInteractInput)
            {
                dialogueUI.NextDialogueLine();
            }

            Debug.Log("Interacting with: ");
        }
        else
        {
            // Show dialogue
            dialogueBox.SetActive(true);
            dialogueOpen = true;

            Debug.Log("Interacting with: ");
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (dialogueBox == null) { return; }

        if (other.CompareTag("Player") && dialogueOpen)
        {
            // Hide dialogue
            dialogueBox.SetActive(false);
            dialogueOpen = false;
        }
    }
}