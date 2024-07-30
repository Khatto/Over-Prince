using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public ShadowedText dialogueTextShadow;
    public Queue<Dialogue> dialogueSequence;
    public Dialogue currentDialogue;
    public DialogueState state = DialogueState.NotStarted;
    public DialogueDisplayMode dialogueDisplayMode = DialogueDisplayMode.Single;
    public Vector2 dialogueDefaultPosition = new Vector2(0.0f, 0.0f);

    public static class DialogueManagerConstants 
    {
        public const float dialogueFadeTime = 0.5f;
    }

    void Start()
    {
        dialogueTextShadow.SetupShadowText();
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
        FadeDialogue(FadeType.FadeOut);
        StartCoroutine(SetDialogueStateAfterTime(DialogueState.Finished, DialogueManagerConstants.dialogueFadeTime));
    }

    private void FadeInAndDisplayDialogue(Dialogue dialogue)
    {
        FadeDialogue(FadeType.FadeIn);
        SetDialogueText(dialogue.text);
        StartCoroutine(SetDialogueStateAfterTime(DialogueState.DisplayingDialogue, dialogue.displayTime));
        StartCoroutine(ModifyDialoguePositionBasedOnLineCount());
    }

    private IEnumerator ModifyDialoguePositionBasedOnLineCount() {
        yield return new WaitForSeconds(0.0f);
        SetDialoguePositionBasedOnLines();
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

    private IEnumerator FadeOutThenDisplayDialogue(Dialogue dialogue, float dialogueFadeTime = DialogueManagerConstants.dialogueFadeTime)
    {
        state = DialogueState.FadingDialogueOut;
        FadeDialogue(FadeType.FadeOut);
        yield return new WaitForSeconds(dialogueFadeTime);
        FadeInAndDisplayDialogue(dialogue);
    }

    private void FadeDialogue(FadeType fadeType)
    {
        dialogueTextShadow.StartFadeWithTime(fadeType, DialogueManagerConstants.dialogueFadeTime);
    }

    public bool CanProceedToNextDialogue() {
        return state == DialogueState.DisplayingDialogue;
    }

    /// <summary>
    /// Sets the color of the dialogue text and its shadow if it exists
    /// </summary>
    /// <param name="color"></param>
    public void SetDialogueColor(Color color) {
        dialogueTextShadow.SetColor(color);
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

    /// <summary>
    /// Sets the text of the dialogue to the provided text, and its shadow if it exists
    /// </summary>
    /// <param name="text"></param>
    private void SetDialogueText(string text)
    {
        dialogueTextShadow.SetText(text);
    }

    /// <summary>
    /// Sets the position of the dialogue based on the number of lines in the dialogue text, and its shadow if it exists
    /// </summary>
    private void SetDialoguePositionBasedOnLines() 
    {
        Vector2 newPosition = new Vector2(
            dialogueTextShadow.rectTransform.anchoredPosition.x, // TODO: Determine if we want to use 
            dialogueDefaultPosition.y - ((dialogueTextShadow.textInfo.lineCount % 2) * dialogueTextShadow.fontSize)
        );
        dialogueTextShadow.SetDialoguePositionBasedOnLines(newPosition);
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