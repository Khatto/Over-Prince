using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    private struct FileLobbyIntroStageManagerConstants {
        public const float dialogueDelay = 1.0f;
    }

    public void Start() {
        base.Start();
        cameraZoom.StartZoom();
    }

    public void Update() {
        base.Update();
        switch(fileLobbyState) {
            case FileLobbyIntroStageState.Dialogue:
                if (dialogueManager.state == DialogueState.Finished) {
                    PerformSceneAction(FileLobbyIntroStageState.IntroduceControls);
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
                player.GetComponent<PlayerController>().SetControlsActive(true);
                cameraFollow.enabled = true;
                cameraFollow.active = true;
                instructionManager.DisplayInstructions(InstructionConstants.ControlInstructions.GetInstructionsBasedOnDevice());
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
    IntroduceControls
}