using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class FileLobbyIntroStageManager : GameplayScene, IAnimationEventListener, IHurtboxTriggerListener
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
    public BattleManager battleManager;
    public InputActionAsset actions;
    private InputAction moveAction;

    public Enemy firstTriangleEnemy;

    public float testForce = 1000;

    private struct FileLobbyIntroStageManagerConstants {
        public const float dialogueDelay = 1.0f;
        public const float panCameraDelay = 0.5f;
        public const float panCameraDuration = 0.5f;
        public const float panCameraToTargetThreshold = 0.1f;
        public const float panCameraToMonsterCameraZoomStartSize = 5.0f;
        public const float panCameraToMonsterCameraZoomEndSize = 3.69f;
        public const float tutorialKnockbackForce = 1000.0f;
        public static Range2D battleCameraFollowMaxRange = new Vector2(0.3f, 17f);
        public static Range2D postBattleCameraFollow = new Vector2(0.3f, 20f); 
    }

    public override void Start() {
        base.Start();
        cameraZoom.StartZoom();
        moveAction = actions.FindActionMap(PlayerController.PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).FindAction(PlayerController.PlayerControllerConstants.InputKeyNames.Move);
        firstTriangleEnemy.GetComponentInChildren<HurtboxManager>().SetListener(this);
        battleManager.Setup(player.GetComponent<Player>(), this, new List<Enemy> { firstTriangleEnemy });
    }

    public override void Update() {
        base.Update();
        switch(fileLobbyState) {
            case FileLobbyIntroStageState.Dialogue:
                if (dialogueManager.IsFinished()) {
                    PerformSceneAction(FileLobbyIntroStageState.IntroduceControls);
                }
                break;
            case FileLobbyIntroStageState.IntroduceControls:
                if (instructionManager.state == InstructionManagerState.WaitingForUserInput && PlayerIsMoving()) {
                    PerformSceneAction(FileLobbyIntroStageState.NavigateToMonster);
                }
                break;
            case FileLobbyIntroStageState.BattleIntroSceneWaitForDialogueCompletion:
                if (dialogueManager.IsFinished()) {
                    // dialogueManager.Reset(); // TODO - Test if this is a better (cleaner) place than in the PerformSceneAction(BattleIntroScene)
                    PerformSceneAction(FileLobbyIntroStageState.PanCameraToMonster);
                }
                break;
            case FileLobbyIntroStageState.PanCameraToMonster:
                if (WithinPanCameraToTargetThreshold(firstTriangleEnemy.transform)) {
                    PerformSceneAction(FileLobbyIntroStageState.ExplainBattle);
                }
                break;
            
            case FileLobbyIntroStageState.ExplainBattle:            
                if (dialogueManager.IsFinished()) {
                    PerformSceneAction(FileLobbyIntroStageState.ReturnCameraBeforeBattleTutorial);
                }
                break;
            case FileLobbyIntroStageState.ReturnCameraBeforeBattleTutorial:
                if (WithinPanCameraToTargetThreshold(player.transform)) {
                    PerformSceneAction(FileLobbyIntroStageState.BattleTutorial);
                }
                break;
            case FileLobbyIntroStageState.FinishDialogueBeforeBattle:
                if (dialogueManager.IsFinished()) {
                    PerformSceneAction(FileLobbyIntroStageState.StartBattle);
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
                DisplayMapProceedIndicator(true);
                StartCoroutine(DelayedAction(InstructionConstants.instructionFadeTime, () => {
                    playerController.SetControlsActive(true);
                }));
                break;
            case FileLobbyIntroStageState.NavigateToMonster:
                instructionManager.HideInstructions();
                DisplayMapProceedIndicator(false);
                break;
            case FileLobbyIntroStageState.BattleIntroScene:
                // dialogueManager.Reset();
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
            case FileLobbyIntroStageState.ReturnCameraBeforeBattleTutorial:
                StartCoroutine(PanCameraBackToPlayer());
                break;
            case FileLobbyIntroStageState.BattleTutorial:
                playerController.enabled = true;
                playerController.SetControlsActive(true);
                instructionManager.DisplayInstructions(InstructionConstants.BasicAttackInstruction.GetInstructionsBasedOnDevice());
                cameraFollow.target = player.transform;
                cinematicFrameManager.ExitFrames();
                break;
            case FileLobbyIntroStageState.PostEnemyHit:
                StartCoroutine(PostEnemyHit());
                break;
            case FileLobbyIntroStageState.StartBattle:
                StartCoroutine(StartBattle());
                break;
        }
    }

    public bool PlayerIsMoving() {
        return Mathf.Abs(moveAction.ReadValue<Vector2>().y) > 0 || Mathf.Abs(moveAction.ReadValue<Vector2>().x) > 0;
    }

    public bool WithinPanCameraToTargetThreshold(Transform targetTransform) {
        return Mathf.Abs(Camera.main.transform.position.x - targetTransform.position.x) <= FileLobbyIntroStageManagerConstants.panCameraToTargetThreshold;
    }

    public IEnumerator PanCameraToMonster() {
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.panCameraDelay);
        cinematicFrameManager.SetFramesToFollowContinuously();
        cameraZoom.SetZoomAndStart(
            FileLobbyIntroStageManagerConstants.panCameraToMonsterCameraZoomStartSize,
            FileLobbyIntroStageManagerConstants.panCameraToMonsterCameraZoomEndSize, 
            FileLobbyIntroStageManagerConstants.panCameraDuration
        );
        cameraFollow.followSpeed = 0.02f;
        cameraFollow.target = firstTriangleEnemy.transform;
    }

    public IEnumerator PanCameraBackToPlayer() {
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.panCameraDelay);
        cameraZoom.SetZoomAndStart(
            FileLobbyIntroStageManagerConstants.panCameraToMonsterCameraZoomEndSize,
            FileLobbyIntroStageManagerConstants.panCameraToMonsterCameraZoomStartSize, 
            FileLobbyIntroStageManagerConstants.panCameraDuration
        );
        cameraFollow.target = player.transform;
    }

    public IEnumerator PostEnemyHit() {
        instructionManager.HideInstructions();
        cinematicFrameManager.EnterFrames();
        playerController.enabled = false;
        playerController.StopMovement();
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.dialogueDelay);
        dialogueManager.DisplayDialogues(DialogueConstants.FieldLobbyIntro.BattlePostEnemyHit.dialogues);
        PerformSceneAction(FileLobbyIntroStageState.FinishDialogueBeforeBattle);
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

    public IEnumerator StartBattle() {
        cinematicFrameManager.ExitFrames();
        cameraFollow.cameraRangeX = FileLobbyIntroStageManagerConstants.battleCameraFollowMaxRange;
        yield return new WaitForSeconds(cinematicFrameManager.GetMovementTime());
        playerController.enabled = true;
        firstTriangleEnemy.SetToMaxHP();
        firstTriangleEnemy.specialBattleType = SpecialCharacterType.None;
        battleManager.StartBattle();
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
        if (gameEvent == GameEvent.AlternativeBattleIntro) {
            instructionManager.HideInstructions();
            PerformSceneAction(FileLobbyIntroStageState.StartBattle);
        }
    }

    public void OnHurtboxTriggerEnter(Character character) {
        if (character is Enemy && fileLobbyState == FileLobbyIntroStageState.BattleTutorial) {
            PerformSceneAction(FileLobbyIntroStageState.PostEnemyHit);
            Vector2 knockback = new Vector2(FileLobbyIntroStageManagerConstants.tutorialKnockbackForce, 0);
            firstTriangleEnemy.rigidBody.AddForce(knockback, ForceMode2D.Impulse);
        }
    }

    public void OnHurtboxTriggerExit(Character character) {
        /* No action needed */
    }

    public override void OnBattleComplete()
    {
        base.OnBattleComplete();
        cameraFollow.cameraRangeX = FileLobbyIntroStageManagerConstants.postBattleCameraFollow;
        DisplayMapProceedIndicator(true);
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
    ReturnCameraBeforeBattleTutorial,
    BattleTutorial,
    PostEnemyHit,
    FinishDialogueBeforeBattle,
    StartBattle,
    FinishedBattle,
    NavigateTowardsEnd
}