using System.Collections;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    public ShadowedText instructionText;
    private ChangeColor instructionChangeColor;
    public InstructionManagerState state = InstructionManagerState.NotStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instructionText.SetupShadowText();
        instructionChangeColor = instructionText.GetComponent<ChangeColor>();
    }

    public void DisplayInstructions(string instructions)
    {
        instructionText.SetText(instructions);
        state = InstructionManagerState.FadingIn;
        StartCoroutine(DisplayInstructions());
    }

    private IEnumerator DisplayInstructions()
    {
        instructionText.StartFadeWithTime(FadeType.FadeIn, InstructionConstants.instructionFadeTime);
        yield return new WaitForSeconds(InstructionConstants.instructionFadeTime);
        instructionChangeColor.StartChangingColor();
        state = InstructionManagerState.WaitingForUserInput;
    }

    private IEnumerator FadeOutInstructions()
    {
        instructionText.StartFadeWithTime(FadeType.FadeOut, InstructionConstants.instructionFadeTime);
        yield return new WaitForSeconds(InstructionConstants.instructionFadeTime);
        state = InstructionManagerState.Finished;
    }
}

public enum InstructionManagerState
{
    NotStarted,
    FadingIn,
    Waiting,
    WaitingForUserInput,
    FadingOut,
    Finished
}