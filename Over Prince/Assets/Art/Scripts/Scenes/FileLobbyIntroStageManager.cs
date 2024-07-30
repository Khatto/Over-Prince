using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileLobbyIntroStageManager : GameplayScene
{
    public FileLobbyIntroStageState state = FileLobbyIntroStageState.StandingUp;
    public Animator protagLyingAnimator;
    public GameObject player;

    public override void StartSceneEntry() {
        base.StartSceneEntry();
    }
}

public enum FileLobbyIntroStageState {
    StandingUp,
    Dialogue,
    IntroduceControls
}