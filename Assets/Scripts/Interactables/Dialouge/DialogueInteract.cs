
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
        Debug.Log("Interacting with: " + gameObject.name);
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
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player") && dialogueOpen)
        {
            dialogueUI.HideDialogue();
            dialogueOpen = false;
        }
    }
}