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

    public Enemy firstTriangleEnemy;

    private struct FileLobbyIntroStageManagerConstants {
        public const float dialogueDelay = 1.0f;
        public const float panCameraToMonsterDelay = 0.5f;
        public const float panCameraToMonsterThreshold = 0.1f;
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
                    PerformSceneAction(FileLobbyIntroStageState.NavigateToMonster);
                }
                break;
            case FileLobbyIntroStageState.BattleIntroSceneWaitForDialogueCompletion:
                if (dialogueManager.state == DialogueState.Finished) {
                    // dialogueManager.Reset(); // TODO - Test if this is a better (cleaner) place than in the PerformSceneAction(BattleIntroScene)
                    PerformSceneAction(FileLobbyIntroStageState.PanCameraToMonster);
                }
                break;
            case FileLobbyIntroStageState.PanCameraToMonster:
                if (Mathf.Abs(Camera.main.transform.position.x - firstTriangleEnemy.transform.position.x) <= FileLobbyIntroStageManagerConstants.panCameraToMonsterThreshold) {
                    PerformSceneAction(FileLobbyIntroStageState.ExplainBattle);
                }
                break;
            case FileLobbyIntroStageState.ExplainBattle:            
                if (dialogueManager.state == DialogueState.Finished) {
                    dialogueManager.Reset();
                    PerformSceneAction(FileLobbyIntroStageState.BattleTutorial);
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
                    //dialogueManager.DisplayDialogues(DialogueConstants.TestDialogue.dialogues);
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
            case FileLobbyIntroStageState.BattleIntroScene:
                dialogueManager.Reset();
                playerController.StopMovement();
                playerController.enabled = false;
                StartCoroutine(BattleIntroScene());
                break;
            case FileLobbyIntroStageState.PanCameraToMonster:
                StartCoroutine(PanCameraToMonster());
                break;
            case FileLobbyIntroStageState.ExplainBattle:
                dialogueManager.DisplayDialogues(DialogueConstants.FieldLobbyIntro.BattleIntroDialoguePartTwo.dialogues);
                break;
            case FileLobbyIntroStageState.BattleTutorial:
                playerController.enabled = true;
                playerController.SetControlsActive(true);
                instructionManager.DisplayInstructions(InstructionConstants.BasicAttackInstruction.GetInstructionsBasedOnDevice());
                firstTriangleEnemy.EnterState(CharacterState.Idle);
                firstTriangleEnemy.hpBar.DisplayHPBar();
                cameraFollow.target = player.transform;
                cinematicFrameManager.ExitFrames();
                break;
        }
    }

    public IEnumerator PanCameraToMonster() {
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.panCameraToMonsterDelay);
        cameraFollow.followSpeed = 0.05f;
        cameraFollow.target = firstTriangleEnemy.transform;
        cameraFollow.enabled = true;
        cameraFollow.active = true;
    }

    public IEnumerator BattleIntroScene() {
        float cinematicFrameManagerMovementTime = cinematicFrameManager.GetMovementTime();
        yield return new WaitForSeconds(cinematicFrameManagerMovementTime * 0.5f);
        cinematicFrameManager.EnterFrames();
        yield return new WaitForSeconds(cinematicFrameManagerMovementTime);
        dialogueManager.DisplayDialogues(DialogueConstants.FieldLobbyIntro.BattleIntroDialoguePartOne.dialogues);
        yield return new WaitForSeconds(DialogueManager.DialogueManagerConstants.dialogueFadeTime);
        PerformSceneAction(FileLobbyIntroStageState.BattleIntroSceneWaitForDialogueCompletion);
    }

    private IEnumerator DelayedAction(float delay, System.Action action) {
        yield return new WaitForSeconds(delay);
        action();
    }

    public override void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent == GameEvent.BattleIntro) {
            PerformSceneAction(FileLobbyIntroStageState.BattleIntroScene);
        }
    }
}

public enum FileLobbyIntroStageState {
    StandingUp,
    Dialogue,
    IntroduceControls,
    NavigateToMonster,
    BattleIntroScene,
    BattleIntroSceneWaitForDialogueCompletion,
    PanCameraToMonster,
    ExplainBattle,
    BattleTutorial
}