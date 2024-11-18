using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FileLobbyIntroStageManager : GameplayScene, IAnimationEventListener, IHurtboxTriggerListener, IDialogueTraitListener
{
    public FileLobbyIntroStageState fileLobbyState = FileLobbyIntroStageState.StandingUp;
    public Player player;
    public Animator protagLyingAnimator;
    public GameObject playerTurnSideToFront;
    public CameraZoom cameraZoom;
    public CameraFollow cameraFollow;
    public DialogueManager dialogueManager;
    public CinematicFrameManager cinematicFrameManager;
    public InstructionManager instructionManager;
    public BattleManager battleManager;
    public ChoiceManager choiceManager;
    public InputActionAsset actions;
    private InputAction moveAction;

    public Enemy firstTriangleEnemy;
    public GameEventInstance alternativeBattleStart;

    public GameObject rightFloorBorder;
    public Character hoodedBoy;
    public SimpleMovement hoodedBoySimpleMovement;

    public SimpleMovement elevatorAppearMaskMovement;
    public ParticleSystem elevatorAppearParticles;
    public Fade[] elevatorAppearFadeElements;
    public ParticleSystem shineBurstParticles;
    public Vector3 moveToElevatorPosition;
    public float moveToElevatorSpeed;

    public float testForce = 1000;

    private struct FileLobbyIntroStageManagerConstants {
        public const float dialogueDelay = 1.0f;
        public const float panCameraDelay = 0.5f;
        public const float panCameraDuration = 0.5f;
        public const float panCameraToTargetThreshold = 0.1f;
        public const float panCameraToTargetCameraZoomStartSize = 5.0f;
        public const float panCameraToMonsterCameraZoomEndSize = 3.69f;
        public const float panCameraToHoodedBoyZoomEndSize = 4.5f;
        public const float zoomCameraOutToElevatorSize = 6.78f;
        public const float zoomCameraToElevatorDuration = 1.5f;
        public const float delayBeforeAppearElevator = 0.25f;
        public const float appearElevatorDelay = 2.0f;
        public const float appearElevatorFlashFadeOut = 0.125f;
        public const float tutorialKnockbackForce = 1000.0f;
        public static Range2D battleCameraFollowMaxRange = new Vector2(0.3f, 21.6f);
        public static Range2D postBattleCameraFollow = new Vector2(0.3f, 20f); 
        public const float postBattleDialogueDelay = 0.25f;
        public const float hoodedBoyMaxCameraRange = 62.0f;

        public const float cameraMoveToTargetFollowSpeed = 0.02f;
        public const float cameraDefaultFollowSpeed = 3.0f;

        public const float hiddenBoyMoveDuration = 3.0f;
        public static Vector2 hiddenBoyMoveVector = new Vector2(0.0f, 1.0f);

        public const float zoomCameraMovingToElevator = 4.70f;
    }

    public override void Start() {
        base.Start();
        cameraZoom.StartZoom();
        moveAction = actions.FindActionMap(PlayerController.PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).FindAction(PlayerController.PlayerControllerConstants.InputKeyNames.Move);
        firstTriangleEnemy.GetComponentInChildren<HurtboxManager>().SetListener(this);
        battleManager.Setup(player.GetComponent<Player>(), this, new List<Enemy> { firstTriangleEnemy });
        dialogueManager.dialogueTraitListener = this;
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
            case FileLobbyIntroStageState.PostBattleDialogue:
                if (dialogueManager.IsFinished()) {
                    PerformSceneAction(FileLobbyIntroStageState.NavigateTowardsEnd);
                }
                break;
            case FileLobbyIntroStageState.NavigateTowardsEnd:
                if (cameraFollow.followSpeed != FileLobbyIntroStageManagerConstants.cameraDefaultFollowSpeed && WithinPanCameraToTargetThreshold(player.transform)) {
                    cameraFollow.followSpeed = FileLobbyIntroStageManagerConstants.cameraDefaultFollowSpeed;
                }
                break;
            case FileLobbyIntroStageState.PanCameraToHoodedBoy:
                if (WithinPanCameraToTargetThreshold(hoodedBoy.transform)) {
                    PerformSceneAction(FileLobbyIntroStageState.HoodedBoyDialogue);
                }
                break;
            case FileLobbyIntroStageState.HoodedBoyDialogue:
                if (dialogueManager.IsFinished()) {
                    PerformSceneAction(FileLobbyIntroStageState.AppearElevator);
                }
                break;
            case FileLobbyIntroStageState.FinalDiscussion:
                if (dialogueManager.IsFinished()) {
                    hoodedBoy.organicMouth.Speak(false);
                    PerformSceneAction(FileLobbyIntroStageState.MoveCloserToElevator);
                }
                break;
            case FileLobbyIntroStageState.MoveCloserToElevator:
                if (player.transform.position.x != moveToElevatorPosition.x || player.transform.position.y != moveToElevatorPosition.y) {
                    Vector3 moveTowardsValue = Vector3.MoveTowards(player.transform.localPosition, moveToElevatorPosition, Time.deltaTime * moveToElevatorSpeed);
                    player.transform.localPosition = moveTowardsValue;
                    player.animator.SetInteger(Constants.AnimationKeys.MoveSpeed, 1);
                } else {
                    PerformSceneAction(FileLobbyIntroStageState.ProceedConfirmation);
                    player.animator.SetInteger(Constants.AnimationKeys.MoveSpeed, 0);
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
                player.gameObject.SetActive(true);
                choiceManager.SetPlayer(player);
                PerformSceneAction(FileLobbyIntroStageState.Dialogue);
                break;
        }
    }

    #region Perform Scene Action
    public void PerformSceneAction(FileLobbyIntroStageState newState) { // TODO: Change this private, it's only Public for Testing
        fileLobbyState = newState;
        switch (fileLobbyState) {
            case FileLobbyIntroStageState.StandingUp:
                protagLyingAnimator.SetTrigger(Constants.AnimationKeys.Start);
                break;
            case FileLobbyIntroStageState.Dialogue:
                cinematicFrameManager.ExitFrames();
                StartCoroutine(DelayedAction(FileLobbyIntroStageManagerConstants.dialogueDelay, () => {
                    musicInfoManager.DisplayMusicInfo();
                    //dialogueManager.DisplayDialogues(DialogueConstants.FieldLobbyIntro.PartOne.dialogues);
                    dialogueManager.DisplayDialogues(DialogueConstants.TestDialogue.dialogues);
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
                playerController.StopMovement();
                playerController.enabled = false;
                StartCoroutine(BattleIntroScene());
                break;
            case FileLobbyIntroStageState.PanCameraToMonster:
                StartCoroutine(PanCameraToTarget(firstTriangleEnemy.gameObject, FileLobbyIntroStageManagerConstants.panCameraToMonsterCameraZoomEndSize));
                break;
            case FileLobbyIntroStageState.ExplainBattle:
                dialogueManager.DisplayDialogues(DialogueConstants.FieldLobbyIntro.BattleIntroDialoguePartTwo.dialogues);
                break;
            case FileLobbyIntroStageState.ReturnCameraBeforeBattleTutorial:
                StartCoroutine(PanCameraBackToPlayer(FileLobbyIntroStageManagerConstants.panCameraToMonsterCameraZoomEndSize));
                break;
            case FileLobbyIntroStageState.BattleTutorial:
                playerController.enabled = true;
                playerController.SetControlsActive(true);
                instructionManager.DisplayInstructions(InstructionConstants.BasicAttackInstruction.GetInstructionsBasedOnDevice());
                cameraFollow.followSpeed = FileLobbyIntroStageManagerConstants.cameraDefaultFollowSpeed;
                cinematicFrameManager.ExitFrames();
                break;
            case FileLobbyIntroStageState.PostEnemyHit:
                StartCoroutine(PostEnemyHit());
                break;
            case FileLobbyIntroStageState.StartBattle:
                StartCoroutine(StartBattle());
                break;
            case FileLobbyIntroStageState.FinishedBattle:
                dialogueManager.Reset(); // TODO - See why this is working so differently from the other Dialogues
                playerController.enabled = false;
                playerController.SetControlsActive(false);
                StartCoroutine(StartPostBattleDialogue());
                PerformSceneAction(FileLobbyIntroStageState.PostBattleDialogue);
                break;
            case FileLobbyIntroStageState.NavigateTowardsEnd:
                playerController.enabled = true;
                playerController.SetControlsActive(true);
                rightFloorBorder.SetActive(false);
                DisplayMapProceedIndicator(true);
                cameraFollow.followSpeed = FileLobbyIntroStageManagerConstants.cameraMoveToTargetFollowSpeed;
                cameraFollow.cameraRangeX.max = FileLobbyIntroStageManagerConstants.hoodedBoyMaxCameraRange;
                break;
            case FileLobbyIntroStageState.PanCameraToHoodedBoy:
                DisplayMapProceedIndicator(false);
                playerController.StopMovement();
                playerController.enabled = false;
                playerController.SetControlsActive(false); // TODO - Look at all the potentially superfluous enabled/SetControlsActive methods
                hoodedBoy.gameObject.SetActive(true);
                dialogueManager.Reset();

                cinematicFrameManager.SetScaleUpdateFrequency(ScaleUpdateFrequency.OnUpdate);
                StartCoroutine(PanCameraToTarget(hoodedBoy.gameObject, FileLobbyIntroStageManagerConstants.panCameraToHoodedBoyZoomEndSize));
                break;
            case FileLobbyIntroStageState.HoodedBoyDialogue:
                hoodedBoy.organicMouth.Speak(true);
                dialogueManager.DisplayDialogues(DialogueConstants.FieldLobbyIntro.HoodedBoyEncounter.dialogues);
                break;
            case FileLobbyIntroStageState.AppearElevator:
                StartCoroutine(AppearElevator());
                break;
            case FileLobbyIntroStageState.HoodedBoyMove:
                StartCoroutine(HoodedBoyMove());
                break;
            case FileLobbyIntroStageState.FinalDiscussion:
                dialogueManager.Reset();
                dialogueManager.DisplayDialogues(DialogueConstants.FieldLobbyIntro.HoodedBoyFinalMessage.dialogues);
                hoodedBoy.animator.SetBool(Constants.AnimationKeys.Walking, false);
                hoodedBoy.organicMouth.ResetToDefaultPosition();
                hoodedBoy.organicMouth.SetMoving(true);
                break;
            case FileLobbyIntroStageState.ProceedConfirmation:
                StartCoroutine(ProceedConfirmation());
                break;
        }
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
        alternativeBattleStart.gameObject.SetActive(false);
    }

    public IEnumerator StartPostBattleDialogue() {
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.postBattleDialogueDelay);
        dialogueManager.DisplayDialogues(DialogueConstants.FieldLobbyIntro.BattleComplete.dialogues);
    }

    public IEnumerator AppearElevator() {
        hoodedBoy.organicMouth.Speak(false);
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.delayBeforeAppearElevator);
        cameraZoom.SetZoomAndStart(
            FileLobbyIntroStageManagerConstants.panCameraToHoodedBoyZoomEndSize,
            FileLobbyIntroStageManagerConstants.zoomCameraOutToElevatorSize, 
            FileLobbyIntroStageManagerConstants.zoomCameraToElevatorDuration
        );
        elevatorAppearMaskMovement.movementTime = FileLobbyIntroStageManagerConstants.appearElevatorDelay;
        elevatorAppearMaskMovement.Move();
        SimpleMovement elevatorAppearParticlesMovement = elevatorAppearParticles.GetComponent<SimpleMovement>();
        elevatorAppearParticlesMovement.movementTime = FileLobbyIntroStageManagerConstants.appearElevatorDelay;
        elevatorAppearParticlesMovement.Move();
        elevatorAppearParticles.Play();
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.appearElevatorDelay);
        foreach (Fade fade in elevatorAppearFadeElements) {
            fade.StartFadeWithTime(FadeType.FadeOut, FileLobbyIntroStageManagerConstants.appearElevatorFlashFadeOut);
        }
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.appearElevatorFlashFadeOut);
        shineBurstParticles.Play();
        PerformSceneAction(FileLobbyIntroStageState.HoodedBoyMove);
    }

    public IEnumerator HoodedBoyMove() {
        hoodedBoy.organicMouth.SetMoving(false);
        hoodedBoy.animator.SetBool(Constants.AnimationKeys.Walking, true);
        hoodedBoySimpleMovement = hoodedBoy.GetComponent<SimpleMovement>();
        hoodedBoySimpleMovement.SetMovementTime(FileLobbyIntroStageManagerConstants.hiddenBoyMoveDuration);
        hoodedBoySimpleMovement.Move();
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.hiddenBoyMoveDuration);
        PerformSceneAction(FileLobbyIntroStageState.FinalDiscussion);
    }

    public IEnumerator ProceedConfirmation() {
        StartCoroutine(PanCameraBackToPlayer(FileLobbyIntroStageManagerConstants.zoomCameraOutToElevatorSize));
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.panCameraDuration);
        dialogueManager.DisplayDialogues(DialogueConstants.FieldLobbyIntro.ElevatorConfirmation.dialogues);
    }

    #endregion Perform Scene Action

    #region Scene and Camera Visuals
    public bool PlayerIsMoving() {
        return Mathf.Abs(moveAction.ReadValue<Vector2>().y) > 0 || Mathf.Abs(moveAction.ReadValue<Vector2>().x) > 0;
    }

    public bool WithinPanCameraToTargetThreshold(Transform targetTransform) {
        return Mathf.Abs(Camera.main.transform.position.x - targetTransform.position.x) <= FileLobbyIntroStageManagerConstants.panCameraToTargetThreshold;
    }

    public IEnumerator PanCameraToTarget(GameObject target, float zoomEndSize) {
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.panCameraDelay);
        cinematicFrameManager.SetFramesToFollowContinuously();
        cameraZoom.SetZoomAndStart(
            FileLobbyIntroStageManagerConstants.panCameraToTargetCameraZoomStartSize,
            zoomEndSize, 
            FileLobbyIntroStageManagerConstants.panCameraDuration
        );
        cameraFollow.followSpeed = FileLobbyIntroStageManagerConstants.cameraMoveToTargetFollowSpeed;
        cameraFollow.target = target.transform;
    }

    public IEnumerator PanCameraBackToPlayer(float zoomEndSize) { // TODO - Consolidate this with PanCameraToTarget
        yield return new WaitForSeconds(FileLobbyIntroStageManagerConstants.panCameraDelay);
        cameraZoom.SetZoomAndStart(
            zoomEndSize,
            FileLobbyIntroStageManagerConstants.panCameraToTargetCameraZoomStartSize, 
            FileLobbyIntroStageManagerConstants.panCameraDuration
        );
        cameraFollow.target = player.transform;
    }
    #endregion Scene and Camera Visuals

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
        if (gameEvent == GameEvent.HoodedBoyEncounter) {
            PerformSceneAction(FileLobbyIntroStageState.PanCameraToHoodedBoy);
        }
    }

    public override void OnBattleComplete()
    {
        base.OnBattleComplete();
        PerformSceneAction(FileLobbyIntroStageState.FinishedBattle);
    }

    public void OnTrait(DialogueTrait trait)
    {
        switch (trait) {
            case DialogueTrait.StopSpeaking:
                hoodedBoy.organicMouth.Speak(false);
                break;
            case DialogueTrait.StartSpeaking:
                hoodedBoy.organicMouth.Speak(true);
                break;
            case DialogueTrait.HideCinematicBars:
                break;
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
    PostBattleDialogue,
    NavigateTowardsEnd,
    PanCameraToHoodedBoy,
    HoodedBoyDialogue,
    AppearElevator,
    HoodedBoyMove,
    FinalDiscussion,
    MoveCloserToElevator,
    ProceedConfirmation,
    RefusalEncouragement,
    ProceedIntoElevator,
    CloseDoorAndLightArrow,
}