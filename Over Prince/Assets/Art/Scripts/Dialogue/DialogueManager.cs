using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    private Fade dialogueFade;

    public Queue<Dialogue> dialogueSequence;
    public Dialogue currentDialogue;
    public DialogueState state = DialogueState.NotStarted;
    public DialogueDisplayMode dialogueDisplayMode = DialogueDisplayMode.Single;
    public Vector2 dialogueDefaultPosition = new Vector2(0.0f, 0.0f);

    private struct DialogueManagerConstants {
        public const float dialogueFadeTime = 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueFade = dialogueText.GetComponent<Fade>();
    }

    public void DisplayDialogues(Dialogue[] dialogues)
    {
        dialogueDisplayMode = DialogueDisplayMode.Sequence;
        dialogueSequence = new Queue<Dialogue>(dialogues);
        DisplayNextQueuedDialogue();
    }

    public void DisplayDialogue(Dialogue dialogue)
    {
        switch (state)
        {
            case DialogueState.NotStarted:
            case DialogueState.Finished:
                state = DialogueState.FadingDialogueIn;
                FadeInAndDisplayDialogue(dialogue);
                break;
            case DialogueState.FadingDialogueIn:
                break;
            case DialogueState.DisplayingDialogue:
                StartCoroutine(FadeOutThenDisplayDialogue(dialogue));
                break;
            case DialogueState.FadingDialogueOut:
                break;
        }
    }

    public void HideDialogue()
    {   state = DialogueState.FadingDialogueOut;
        dialogueFade.StartFadeWithTime(FadeType.FadeOut, DialogueManagerConstants.dialogueFadeTime);
        StartCoroutine(SetDialogueStateAfterTime(DialogueState.Finished, DialogueManagerConstants.dialogueFadeTime));
    }

    /// <summary>
    /// Displays the next dialogue in the queue in the dialogueSequence if one exists
    /// </summary>
    private void DisplayNextQueuedDialogue()
    {
        if (dialogueSequence.Count > 0) {
            DisplayDialogue(dialogueSequence.Dequeue());
        } else {
            dialogueDisplayMode = DialogueDisplayMode.Single;
            HideDialogue();
        }
    }

    private void FadeInAndDisplayDialogue(Dialogue dialogue)
    {
        dialogueFade.StartFadeWithTime(FadeType.FadeIn, DialogueManagerConstants.dialogueFadeTime);
        dialogueText.text = dialogue.text;
        StartCoroutine(SetDialogueStateAfterTime(DialogueState.DisplayingDialogue, dialogue.displayTime));
        StartCoroutine(ModifyDialoguePositionBasedOnLineCount());
    }

    private IEnumerator ModifyDialoguePositionBasedOnLineCount() {
        yield return new WaitForSeconds(0.0f);
        Debug.Log("Number of lines for Text Mesh Pro: " + dialogueText.text + " = " + dialogueText.textInfo.lineCount);
        dialogueText.rectTransform.anchoredPosition = new Vector2(
            dialogueText.rectTransform.anchoredPosition.x, // TODO: Determine if we want to use 
            dialogueDefaultPosition.y - ((dialogueText.textInfo.lineCount % 2) * (dialogueText.fontSize))
        );

    }

    private IEnumerator SetDialogueStateAfterTime(DialogueState state, float displayTime)
    {
        yield return new WaitForSeconds(displayTime + DialogueManagerConstants.dialogueFadeTime);
        this.state = state;
        if (dialogueDisplayMode == DialogueDisplayMode.Sequence)
        {
            DisplayNextQueuedDialogue();
        }
    }

    private IEnumerator FadeOutThenDisplayDialogue(Dialogue dialogue)
    {
        state = DialogueState.FadingDialogueOut;
        dialogueFade.StartFadeWithTime(FadeType.FadeOut, DialogueManagerConstants.dialogueFadeTime);
        yield return new WaitForSeconds(DialogueManagerConstants.dialogueFadeTime);
        FadeInAndDisplayDialogue(dialogue);
    }

    public bool CanProceedToNextDialogue() {
        return state == DialogueState.DisplayingDialogue;
    }

    public void SetDialogueColor(Color color) {
        dialogueText.color = color;
    }


}

public enum DialogueState {
    NotStarted,
    FadingDialogueIn,
    DisplayingDialogue,
    FadingDialogueOut,
    Finished
}

public enum DialogueDisplayMode {
    Single,
    Sequence
}