using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    private Fade dialogueFade;

    public TextMeshProUGUI shadowText;
    private Fade shadowFade;
    public Vector2 shadowOffset = new Vector2(2.0f, -2.0f);

    public Queue<Dialogue> dialogueSequence;
    public Dialogue currentDialogue;
    public DialogueState state = DialogueState.NotStarted;
    public DialogueDisplayMode dialogueDisplayMode = DialogueDisplayMode.Single;
    public Vector2 dialogueDefaultPosition = new Vector2(0.0f, 0.0f);

    public static class DialogueManagerConstants {
        public const float dialogueFadeTime = 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueFade = dialogueText.GetComponent<Fade>();
        SetupDialogueShadow();
    }

    private void SetupDialogueShadow() {
        if (
            dialogueText.GetComponent<TextComponent>() != null
            && dialogueText.GetComponent<TextComponent>().shadowText != null
        )
        {
            shadowText = dialogueText.GetComponent<TextComponent>().shadowText;
            if (shadowText != null) {
                shadowFade = shadowText.GetComponent<Fade>();
            }
        }
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
        dialogueFade.StartFadeWithTime(fadeType, DialogueManagerConstants.dialogueFadeTime);
        if (DialogueHasShadow())
        {
            shadowFade.StartFadeWithTime(fadeType, DialogueManagerConstants.dialogueFadeTime);
        }
    }

    public bool CanProceedToNextDialogue() {
        return state == DialogueState.DisplayingDialogue;
    }

    /// <summary>
    /// Sets the color of the dialogue text and its shadow if it exists
    /// </summary>
    /// <param name="color"></param>
    public void SetDialogueColor(Color color) {
        dialogueText.color = color;
        if (DialogueHasShadow()) {
            shadowText.color = color;
        }
    }

    /// <summary>
    /// Returns true if the dialogue has a shadow text object, false otherwise
    /// </summary>
    /// <returns></returns>
    public bool DialogueHasShadow() {
        return shadowText != null;
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
        dialogueText.SetText(text);
        if (DialogueHasShadow())
        {
            shadowText.SetText(text);
        }
    }

    /// <summary>
    /// Sets the position of the dialogue based on the number of lines in the dialogue text, and its shadow if it exists
    /// </summary>
    private void SetDialoguePositionBasedOnLines() {
        Vector2 newPosition = new Vector2(
            dialogueText.rectTransform.anchoredPosition.x, // TODO: Determine if we want to use 
            dialogueDefaultPosition.y - ((dialogueText.textInfo.lineCount % 2) * dialogueText.fontSize)
        );
        dialogueText.rectTransform.anchoredPosition = newPosition;
        if (DialogueHasShadow())
        {
            shadowText.rectTransform.anchoredPosition = newPosition + shadowOffset;
        }
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