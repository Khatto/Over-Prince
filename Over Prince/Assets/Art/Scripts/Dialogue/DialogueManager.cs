using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public ShadowedText dialogueTextShadow;
    public Queue<Dialogue> dialogueSequence;
    public Dialogue currentDialogue;
    public DialogueState state = DialogueState.NotStarted;
    public DialogueDisplayMode dialogueDisplayMode = DialogueDisplayMode.Single;
    public Vector2 dialogueDefaultPosition = new Vector2(0.0f, 0.0f);
    public SpriteRenderer dialogueBackground;
    public bool useDialogueBackground = false;

    public InputActionAsset actions;
    private InputAction proceedInputAction;
    public Fade proceedIndicatorFade;

    public static class DialogueManagerConstants 
    {
        public const float dialogueFadeTime = 0.5f;

        internal static class DialogueControls {
            internal const string ActionMapName = "dialogue";
            internal const string Proceed = "proceed";
        }
    }

    void Start()
    {
        dialogueTextShadow.SetupShadowText();
        SetupInputActions();
    }

    public void SetupInputActions() {
        proceedInputAction = actions.FindActionMap(DialogueManagerConstants.DialogueControls.ActionMapName).FindAction(DialogueManagerConstants.DialogueControls.Proceed);
        proceedInputAction.performed += (context) => InputPressedDuringDialogue(context);
    }

    public void InputPressedDuringDialogue(InputAction.CallbackContext context) {
        if (state == DialogueState.DisplayingDialogueWaitingForUserInput) {
            DisplayNextQueuedDialogue();
        }
    }

    /// <summary>
    /// Starts the process of displaying Dialogues from the selected array.
    /// </summary>
    public void DisplayDialogues(Dialogue[] dialogues)
    {
        dialogueDisplayMode = DialogueDisplayMode.Sequence;
        dialogueSequence = new Queue<Dialogue>(dialogues);
        if (useDialogueBackground) {
            SetupDialogueBackground();
        }
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
            case DialogueState.DisplayingDialogueWaitingForUserInput:
                StartCoroutine(FadeOutThenDisplayDialogue(dialogue));
                break;
            case DialogueState.FadingDialogueOut:
                break;
        }
    }

    public void HideDialogue()
    {   state = DialogueState.FadingDialogueOut;
        FadeDialogue(FadeType.FadeOut);
        if (useDialogueBackground) {
            dialogueBackground.GetComponent<Fade>().StartFadeWithTime(FadeType.FadeOut, DialogueManagerConstants.dialogueFadeTime);
        }
        StartCoroutine(SetDialogueStateAfterTime(DialogueState.Finished, DialogueManagerConstants.dialogueFadeTime));
    }

    private void FadeInAndDisplayDialogue(Dialogue dialogue)
    {
        FadeDialogue(FadeType.FadeIn);
        SetDialogueText(dialogue.text);
        if (dialogue.IsTimed()) {
            StartCoroutine(SetDialogueStateAfterTime(DialogueState.DisplayingDialogue, dialogue.displayTime));
        } else {
            StartCoroutine(DisplayProceedIndicatorAfterFadeIn());
        }
        
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

    private IEnumerator DisplayProceedIndicatorAfterFadeIn()
    {
        yield return new WaitForSeconds(DialogueManagerConstants.dialogueFadeTime);
        if (proceedIndicatorFade != null) {
            proceedIndicatorFade.StartFadeWithTime(FadeType.FadeIn, DialogueManagerConstants.dialogueFadeTime);
        }
        state = DialogueState.DisplayingDialogueWaitingForUserInput;
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
        if (proceedIndicatorFade != null) {
            proceedIndicatorFade.StartFadeWithTime(FadeType.FadeOut, DialogueManagerConstants.dialogueFadeTime);
        }
    }

    public bool CanProceedToNextDialogue() {
        return state == DialogueState.DisplayingDialogue || state == DialogueState.DisplayingDialogueWaitingForUserInput;
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

    private void SetupDialogueBackground() {
        // TODO - Add a check to see if we've already set this up
        dialogueBackground.GetComponent<AutoAlign>().AdjustVerticalAlignment();
        dialogueBackground.GetComponent<AutoScale>().SetScale();
        dialogueBackground.GetComponent<Fade>().StartFadeWithTime(FadeType.FadeIn, DialogueManagerConstants.dialogueFadeTime);
    }

    private void OnEnable()
    {
        actions.FindActionMap(DialogueManagerConstants.DialogueControls.ActionMapName).Enable();
    }

    private void OnDisable()
    {
        actions.FindActionMap(DialogueManagerConstants.DialogueControls.ActionMapName).Disable();
    }
}

public enum DialogueState {
    NotStarted,
    FadingDialogueIn,
    DisplayingDialogue,
    DisplayingDialogueWaitingForUserInput,
    FadingDialogueOut,
    Finished
}

public enum DialogueDisplayMode {
    Single,
    Sequence
}