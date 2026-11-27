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

        currentDialogueLine = 1;
        UpdateDialogueLine();

        if (totalDialogueLines < 2)
        {
            talkInfo.style.display = DisplayStyle.None;
        }
    }

    public void NextDialogueLine()
    {
        if (currentDialogueLine < totalDialogueLines)
        {
            currentDialogueLine++;
        }
        else if (currentDialogueLine == totalDialogueLines)
        {
            currentDialogueLine = totalDialogueLines;
        }

        UpdateDialogueLine();
    }

    private void UpdateDialogueLine()
    {
        speakerText.text = dialogueLines[currentDialogueLine - 1].speaker;
        dialogueText.text = dialogueLines[currentDialogueLine - 1].dialogue;
    }
}
