using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public ShadowedText dialogueTextShadow;
    public Queue<Dialogue> dialogueSequence;
    public Dialogue currentDialogue;
    public DialogueState state = DialogueState.NotStarted;
    public DialogueDisplayMode dialogueDisplayMode = DialogueDisplayMode.Single;
    public Vector2 dialogueDefaultPosition = new Vector2(0.0f, 0.0f);
    public float dialogueDoubleLineVerticalOffset = 0.0f;
    public SpriteRenderer dialogueBackground;
    public bool useDialogueBackground = false;

    public InputActionAsset actions;
    private InputAction proceedInputAction;
    public Fade proceedIndicatorFade;
    public SpriteRenderer proceedIndicatorSpriteRenderer;
    public MoveBackAndForth proceedIndicatorMovement;
    public Fade proceedIndicatorShadowFade;

    public bool displayingChoices = false;
    public Button choice1Button;
    public Button choice2Button;
    public Button choice3Button;
    public ShadowedText choicePrompt;
    public int numberOfChoices = 0;

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
        SetupShadowedTexts();
        SetupInputActions();
        if (proceedIndicatorFade != null) {
            proceedIndicatorSpriteRenderer = proceedIndicatorFade.GetComponent<SpriteRenderer>();
            proceedIndicatorMovement = proceedIndicatorFade.GetComponent<MoveBackAndForth>();
        }
    }

    public void SetupShadowedTexts() {
        dialogueTextShadow.SetupShadowText();
    }

    public void SetupInputActions() {
        proceedInputAction = actions.FindActionMap(DialogueManagerConstants.DialogueControls.ActionMapName).FindAction(DialogueManagerConstants.DialogueControls.Proceed);
        proceedInputAction.performed += (context) => InputPressedDuringDialogue(context);
    }

    public void InputPressedDuringDialogue(InputAction.CallbackContext context) {
        if (state == DialogueState.DisplayingDialogueWaitingForUserInput && !displayingChoices) {
            ChoiceSelectedOrProceedInitiated();
        }
    }

    private void ChoiceSelectedOrProceedInitiated(bool playAudio = true) {
        DisplayNextQueuedDialogue();
        if (playAudio) SoundManager.instance.PlaySound(SoundType.Confirm);
    }

    /// <summary>
    /// Starts the process of displaying Dialogues from the selected array.
    /// </summary>
    public void DisplayDialogues(Dialogue[] dialogues)
    {
        Reset();
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

    private void FadeInAndDisplayDialogue(Dialogue dialogue)
    {
        displayingChoices = dialogue.IsChoice();
        if (dialogue.choices != null && dialogue.choices.Length > 0) {
            numberOfChoices = dialogue.choices.Length;
        } else {
            numberOfChoices = 0;
        }
        FadeDialogue(FadeType.FadeIn);
        SetDialogueText(dialogue);
        if (dialogue.IsTimed()) {
            StartCoroutine(SetDialogueStateAfterTime(DialogueState.DisplayingDialogue, dialogue.displayTime));
        } else if (!displayingChoices) {
            StartCoroutine(DisplayProceedIndicatorAfterFadeIn());
        }
        
        StartCoroutine(ModifyDialoguePositionBasedOnLineCount());
    }

    public void HideDialogue()
    {   state = DialogueState.FadingDialogueOut;
        FadeDialogue(FadeType.FadeOut);
        if (useDialogueBackground) {
            dialogueBackground.GetComponent<Fade>().StartFadeWithTime(FadeType.FadeOut, DialogueManagerConstants.dialogueFadeTime);
        }
        StartCoroutine(SetDialogueStateAfterTime(DialogueState.Finished, DialogueManagerConstants.dialogueFadeTime));
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
        if (IsFinished())
        {
            if (proceedIndicatorMovement != null) proceedIndicatorMovement.active = false;
            if (proceedIndicatorFade != null) proceedIndicatorFade.StartFadeWithTime(FadeType.FadeOut, DialogueManagerConstants.dialogueFadeTime);
            if (proceedIndicatorShadowFade != null) proceedIndicatorShadowFade.StartFadeWithTime(FadeType.FadeOut, DialogueManagerConstants.dialogueFadeTime);
        }
    }

    private IEnumerator DisplayProceedIndicatorAfterFadeIn()
    {
        yield return new WaitForSeconds(DialogueManagerConstants.dialogueFadeTime);
        if (proceedIndicatorFade != null) {
            proceedIndicatorFade.StartFadeWithTime(FadeType.FadeIn, DialogueManagerConstants.dialogueFadeTime);
            proceedIndicatorShadowFade.StartFadeWithTime(FadeType.FadeIn, DialogueManagerConstants.dialogueFadeTime);
            proceedIndicatorMovement.active = true;
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
        if (!displayingChoices) {
            dialogueTextShadow.StartFadeWithTime(fadeType, DialogueManagerConstants.dialogueFadeTime);
            if (proceedIndicatorFade != null) {
                proceedIndicatorFade.StartFadeWithTime(fadeType, DialogueManagerConstants.dialogueFadeTime);
                proceedIndicatorShadowFade.StartFadeWithTime(fadeType, DialogueManagerConstants.dialogueFadeTime);
            }
        } else {
            FadeChoiceButton(choice1Button, fadeType);
            FadeChoiceButton(choice2Button, fadeType);
            if (numberOfChoices > 2) FadeChoiceButton(choice3Button, fadeType);
            StartCoroutine(ChoiceButtonsReadyForInteraction());
        }
    }

    private void FadeChoiceButton(Button button, FadeType fadeType) {
        button.GetComponent<Fade>().StartFadeWithTime(fadeType, DialogueManagerConstants.dialogueFadeTime);
        button.transform.GetChild(0).GetComponent<Fade>().StartFadeWithTime(fadeType, DialogueManagerConstants.dialogueFadeTime);
        StartCoroutine(SetButtonInteractivity(fadeType));
    }

    public IEnumerator SetButtonInteractivity(FadeType fadeType) {
        if (fadeType == FadeType.FadeIn) yield return new WaitForSeconds(ChoiceConstants.choiceInteractivityDelay);
        choice1Button.interactable = fadeType == FadeType.FadeIn;
        choice2Button.interactable = fadeType == FadeType.FadeIn;
        choice3Button.interactable = fadeType == FadeType.FadeIn;
    }

    public IEnumerator ChoiceButtonsReadyForInteraction() {
        yield return new WaitForSeconds(ChoiceConstants.choiceInteractivityDelay);
        state = DialogueState.DisplayingDialogueWaitingForUserInput;
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
    private void SetDialogueText(Dialogue dialogue)
    {
        if (displayingChoices) {
            SetChoicesText(dialogue);
        } else {
            dialogueTextShadow.SetText(dialogue.text);
        }
    }

    private void SetChoicesText(Dialogue dialogue) {
        choicePrompt.SetText(dialogue.text);
        SetupChoiceButton(choice1Button, dialogue.choices[0]);
        SetupChoiceButton(choice2Button, dialogue.choices[1]);
        if (dialogue.choices.Length > 2) {
            SetupChoiceButton(choice3Button, dialogue.choices[2]);
        }
    }

    private void SetupChoiceButton(Button button, Choice choice) {
        button.GetComponent<ChoiceDataHolder>().choice = choice;
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(choice.text);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(button.transform.position);
            StartCoroutine(OnChoiceSelected(choice, worldPosition));
        });
    }

    private IEnumerator OnChoiceSelected(Choice choice, Vector3 position) {
        ChoiceManager.instance.MakeEmotionChoice(choice, position);
        choice1Button.interactable = false;
        choice2Button.interactable = false;
        if (numberOfChoices > 2) choice3Button.interactable = false;
        SoundManager.instance.PlaySound(SoundType.Confirm);
        yield return new WaitForSeconds(ChoiceConstants.choiceSelectionAnimationDuration);
        ChoiceSelectedOrProceedInitiated(false);
    }

    /// <summary>
    /// Sets the position of the dialogue based on the number of lines in the dialogue text, and its shadow if it exists
    /// </summary>
    private void SetDialoguePositionBasedOnLines() 
    {
        int lineCount = dialogueTextShadow.textInfo.lineCount;
        float verticalOffset = (lineCount % 2) * dialogueTextShadow.fontSize;
        Vector2 newPosition = new Vector2(
            dialogueTextShadow.rectTransform.anchoredPosition.x, // TODO: Determine if we want to use 
            dialogueDefaultPosition.y - verticalOffset + (lineCount > 1 ? dialogueDoubleLineVerticalOffset : 0)
        );
        dialogueTextShadow.SetDialoguePositionBasedOnLines(newPosition);
    }

    private void SetupDialogueBackground() {
        // TODO - Add a check to see if we've already set this up
        dialogueBackground.GetComponent<AutoAlign>().AdjustVerticalAlignment();
        dialogueBackground.GetComponent<AutoScale>().Scale();
        dialogueBackground.GetComponent<Fade>().StartFadeWithTime(FadeType.FadeIn, DialogueManagerConstants.dialogueFadeTime);
    }

    // TODO - Make this automatic?
    public void Reset() {
        state = DialogueState.NotStarted;
        if (proceedIndicatorMovement != null) proceedIndicatorMovement.StopMovement();
        if (proceedIndicatorFade != null) proceedIndicatorFade.transform.localPosition = Vector2.zero;
    }

    public bool IsFinished() {
        return state == DialogueState.Finished;
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