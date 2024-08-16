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
                break;
            case FileLobbyIntroStageState.IntroduceControls:
                break;
        }
    }
}

public enum FileLobbyIntroStageState {
    StandingUp,
    Dialogue,
    IntroduceControls
}