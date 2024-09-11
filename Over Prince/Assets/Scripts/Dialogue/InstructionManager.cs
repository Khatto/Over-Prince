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

    public void HideInstructions()
    {
        state = InstructionManagerState.FadingOut;
        StartCoroutine(FadeOutInstructions());
    }

    private IEnumerator DisplayInstructions()
    {
        instructionText.StartFadeWithTime(FadeType.FadeIn, InstructionConstants.instructionFadeTime);
        yield return new WaitForSeconds(InstructionConstants.instructionFadeTime);
        instructionChangeColor.StartChangingColorWithMode(ChangeColorMode.ChangeColorBackAndForth);
        state = InstructionManagerState.WaitingForUserInput;
    }

    private IEnumerator FadeOutInstructions()
    {
        instructionChangeColor.StopChangingColor();
        instructionText.StartFadeWithTime(FadeType.FadeOut, InstructionConstants.instructionFadeTime);
        yield return new WaitForSeconds(InstructionConstants.instructionFadeTime);
        state = InstructionManagerState.Finished;
    }
}

public enum InstructionManagerState
{
    NotStarted,
    FadingIn,
    WaitingForUserInput,
    FadingOut,
    Finished
}