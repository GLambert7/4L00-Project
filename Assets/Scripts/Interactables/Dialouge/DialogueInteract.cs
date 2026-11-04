
using UnityEngine;

public class DialogueInteract : BaseInteractable
{
    [Header("Dialogue Settings")]
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private string characterName;

    [TextArea(3, 5)]
    [SerializeField] private string dialogue;


    private bool dialogueOpen = false;

    public override void PlayerInteract()
    {
        base.PlayerInteract();

        if (dialogueOpen)
        {
            dialogueUI.HideDialogue();
            dialogueOpen = false;
        }
        else
        {
            dialogueUI.ShowDialogue(characterName, dialogue);
            dialogueOpen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && dialogueOpen)
        {
            dialogueUI.HideDialogue();
            dialogueOpen = false;
        }
    }
}