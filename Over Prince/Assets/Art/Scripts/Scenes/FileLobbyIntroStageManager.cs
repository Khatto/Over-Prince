using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FileLobbyIntroStageManager : GameplayScene, IAnimationEventListener
{
    public FileLobbyIntroStageState fileLobbyState = FileLobbyIntroStageState.StandingUp;
    public GameObject player;
    public Animator protagLyingAnimator;
    public GameObject playerTurnSideToFront;
    public CameraZoom cameraZoom;
    public CameraFollow cameraFollow;
    public DialogueManager dialogueManager;
    public CinematicFrameManager cinematicFrameManager;
    public InstructionManager instructionManager;
    public SpriteRenderer mapProceedIndicator;

    public InputActionAsset actions;
    private InputAction moveAction;

    private struct FileLobbyIntroStageManagerConstants {
        public const float dialogueDelay = 1.0f;
    }

    public override void Start() {
        base.Start();
        cameraZoom.StartZoom();
        moveAction = actions.FindActionMap(PlayerController.PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).FindAction(PlayerController.PlayerControllerConstants.InputKeyNames.Move);
    }

    public override void Update() {
        base.Update();
        switch(fileLobbyState) {
            case FileLobbyIntroStageState.Dialogue:
                if (dialogueManager.state == DialogueState.Finished) {
                    PerformSceneAction(FileLobbyIntroStageState.IntroduceControls);
                }
                break;
            case FileLobbyIntroStageState.IntroduceControls:
                if (Mathf.Abs(moveAction.ReadValue<Vector2>().y) > 0 || Mathf.Abs(moveAction.ReadValue<Vector2>().x) > 0) {
                    Debug.Log("Navigating to monster because Y: " + Mathf.Abs(moveAction.ReadValue<Vector2>().y) + " X: " + Mathf.Abs(moveAction.ReadValue<Vector2>().x));
                    PerformSceneAction(FileLobbyIntroStageState.NavigateToMonster);
                }
                break;
        }
    }

    public override void StartSceneEntry() {
        base.StartSceneEntry();
        protagLyingAnimator.SetTrigger(Constants.AnimationKeys.Start);
    }

    public void OnAnimationEvent(AnimationEvent animationEvent) {
        switch (animationEvent) {
            case AnimationEvent.ProtagLyingToStandingFinished:
                protagLyingAnimator.gameObject.SetActive(false);
                playerTurnSideToFront.SetActive(true);
                break;
            case AnimationEvent.ProtagSideTurnToFrontFinished:
                playerTurnSideToFront.SetActive(false);
                player.SetActive(true);
                
                PerformSceneAction(FileLobbyIntroStageState.Dialogue);
                break;
        }
    }

    private void PerformSceneAction(FileLobbyIntroStageState newState) {
        fileLobbyState = newState;
        switch (fileLobbyState) {
            case FileLobbyIntroStageState.StandingUp:
                protagLyingAnimator.SetTrigger(Constants.AnimationKeys.Start);
                break;
            case FileLobbyIntroStageState.Dialogue:
                cinematicFrameManager.ExitFrames();
                StartCoroutine(DelayedAction(FileLobbyIntroStageManagerConstants.dialogueDelay, () => {
                    musicInfoManager.DisplayMusicInfo();
                    dialogueManager.DisplayDialogues(DialogueConstants.FieldLobbyIntro.PartOne.dialogues);
                }));
                break;
            case FileLobbyIntroStageState.IntroduceControls:
                cameraFollow.enabled = true;
                cameraFollow.active = true;
                instructionManager.DisplayInstructions(InstructionConstants.ControlInstructions.GetInstructionsBasedOnDevice());
                mapProceedIndicator.gameObject.SetActive(true);
                mapProceedIndicator.GetComponent<ChangeColor>().SetColorThenChange(Color.white);
                StartCoroutine(DelayedAction(InstructionConstants.instructionFadeTime, () => {
                    player.GetComponent<PlayerController>().SetControlsActive(true);
                }));
                break;
            case FileLobbyIntroStageState.NavigateToMonster:
                instructionManager.HideInstructions();
                mapProceedIndicator.GetComponent<ChangeColor>().StopChangingColor();
                mapProceedIndicator.GetComponent<Fade>().StartFade(FadeType.FadeOut);
                break;
        }
    }

    private IEnumerator DelayedAction(float delay, System.Action action) {
        yield return new WaitForSeconds(delay);
        action();
    }
}

public enum FileLobbyIntroStageState {
    StandingUp,
    Dialogue,
    IntroduceControls,
    NavigateToMonster
}