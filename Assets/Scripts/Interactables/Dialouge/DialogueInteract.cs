using UnityEngine;
using UnityEngine.UIElements;

public class DialogueInteract : BaseInteractable
{
    [Header("Dialogue Settings")]
    [SerializeField] private GameObject dialogueBox1;

    private bool dialogueOpen = false;

    public override void PlayerInteract()
    {
        base.PlayerInteract();
        if (dialogueBox1 == null) { return; }

        if (dialogueOpen)
        {
            // Hide dialogue
            dialogueBox1.SetActive(false);
            dialogueOpen = false;

            Debug.Log("Interacting with: ");
        }
        else
        {
            // Show dialogue
            dialogueBox1.SetActive(true);
            dialogueOpen = true;

            Debug.Log("Interacting with: ");
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (dialogueBox1 == null) { return; }

        if (other.CompareTag("Player") && dialogueOpen)
        {
            // Hide dialogue
            dialogueBox1.SetActive(false);
            dialogueOpen = false;
        }
    }
}