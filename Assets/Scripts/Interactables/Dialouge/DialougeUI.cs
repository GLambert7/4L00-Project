
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private TMP_Text dialogueText;

    private void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string name, string dialogue)
    {
        if (characterName != null)
        {
            characterName.text = name;
        }

        dialogueText.text = dialogue;
        dialoguePanel.SetActive(true);
    }


    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
