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

    private struct DialogueManagerConstants {
        public const float dialogueFadeTime = 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueFade = dialogueText.GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        
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