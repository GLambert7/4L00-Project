using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueBox : MonoBehaviour
{
    // List of dialogue lines and speakers
    [System.Serializable]
    public class DialogueLine
    {
        public string speaker;
        public string dialogue;
    }

    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    // Referencing speaker and dialogue text boxes in UI Document
    private VisualElement _root;
    private VisualElement talkInfo;
    private Label speakerText;
    private Label dialogueText;

    // Dialogue lines info
    private int currentDialogueLine;
    private int totalDialogueLines;

    private void OnEnable()
    {
        // Element refs
        _root = GetComponent<UIDocument>().rootVisualElement;
        talkInfo = _root.Q<VisualElement>("TalkInfo");
        speakerText = _root.Q<Label>("SpeakerText");
        dialogueText = _root.Q<Label>("DialogueText");

        // Update line count and ensure line is set to first
        totalDialogueLines = dialogueLines.Count;

        ResetDialogue();

        if (totalDialogueLines < 2)
        {
            talkInfo.style.display = DisplayStyle.None;
        }
    }

    private void ResetDialogue()
    {
        currentDialogueLine = 1;
        _root.style.display = DisplayStyle.Flex;
        UpdateDialogueLine();
    }

    public void NextDialogueLine()
    {
        if (currentDialogueLine < totalDialogueLines)
        {
            currentDialogueLine++;
            UpdateDialogueLine();
        }
        else if (currentDialogueLine == totalDialogueLines)
        {
            // Cap dialogue lines
            currentDialogueLine = totalDialogueLines + 1;
            _root.style.display = DisplayStyle.None;
        }
        else if (currentDialogueLine == totalDialogueLines + 1)
        {
            ResetDialogue();
        }
    }

    private void UpdateDialogueLine()
    {
        speakerText.text = dialogueLines[currentDialogueLine - 1].speaker;
        dialogueText.text = dialogueLines[currentDialogueLine - 1].dialogue;
    }
}
