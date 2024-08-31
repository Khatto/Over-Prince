using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MovableCharacterController, IHurtableCharacterController
{
    public bool testMoveSpeed = false;

    public static class PlayerControllerConstants {
        public static class InputKeyNames {
            internal const string PlayerInputActionMapName = "playerInput";
            public const string Move = "move";
            internal const string Sprint = "sprint";
            internal const string Attack1 = "attack1";
            internal const string Attack2 = "attack2";
            internal const string Attack3 = "attack3";
            internal const string Attack4 = "attack4";
            internal const string TestAction = "testAction";
        }
        internal static float joystickSprintThreshold = 0.75f;
        internal static float joystickFadeTime = 0.5f;
    }
    private Player player;

    public InputActionAsset actions;
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction attack1Action;
    private InputAction attack2Action;
    private InputAction attack3Action;
    private InputAction attack4Action;

    private InputAction testAction;

    public Animator animator;

    public PlayerControllerState state = PlayerControllerState.Inactive;

    public Gamepad gamepad;

    public Canvas touchControlsCanvas;
    public Fade joystickBackgroundFade;
    public Fade joystickKnobFade;

    void Start()
    {
        SetupActions();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        gamepad = Gamepad.current;
    }

    void FixedUpdate()
    {
        if (state == PlayerControllerState.Active) {
            Vector2 moveVector = moveAction.ReadValue<Vector2>();
            moveVector.y *= Constants.verticalMovementModifier;
            UpdateSpriteFromMovement(moveVector);
            if (moveVector != Vector2.zero && player.state != CharacterState.Attacking) {
                rigidBody.MovePosition(rigidBody.position + moveVector * (testMoveSpeed ? moveSpeed : PlayerConstants.GetMoveSpeed(player.state)) * Time.fixedDeltaTime);
            }
        }
    }

    void SetupActions() {
        moveAction = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).FindAction(PlayerControllerConstants.InputKeyNames.Move);
        sprintAction = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).FindAction(PlayerControllerConstants.InputKeyNames.Sprint);

        attack1Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).FindAction(PlayerControllerConstants.InputKeyNames.Attack1);
        attack1Action.performed += (context) => OnAttackPressed(context, 0);

        attack2Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).FindAction(PlayerControllerConstants.InputKeyNames.Attack2);
        attack2Action.performed += (context) => OnAttackPressed(context, 1);

        attack3Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).FindAction(PlayerControllerConstants.InputKeyNames.Attack3);
        attack3Action.performed += (context) => OnAttackPressed(context, 2);

        attack4Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).FindAction(PlayerControllerConstants.InputKeyNames.Attack4);
        attack4Action.performed += (context) => OnAttackPressed(context, 3);
        
        testAction = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).FindAction(PlayerControllerConstants.InputKeyNames.TestAction);
        testAction.performed += OnTestButtonPressed;
    }

    public void SetControlsActive(bool active) {
        state = active ? PlayerControllerState.Active : PlayerControllerState.Inactive;
        if (active && SystemInfo.deviceType == DeviceType.Handheld) {
            touchControlsCanvas.enabled = true;
            joystickBackgroundFade.StartFade(FadeType.FadeIn);
            joystickKnobFade.StartFade(FadeType.FadeIn);
        } else if (!active) {
            joystickBackgroundFade.StartFadeWithTime(FadeType.FadeOut, PlayerControllerConstants.joystickFadeTime, () => touchControlsCanvas.enabled = false);
            joystickKnobFade.StartFadeWithTime(FadeType.FadeOut, PlayerControllerConstants.joystickFadeTime);
        }
    }

    void UpdateSpriteFromMovement(Vector2 moveVector) {
        // TODO: (Performance) Check if it's more performant to only transform if we've changed our moveVector speed, or every method call
        if (player.state != CharacterState.Attacking) {
            if (transform.localScale.x > 0.0 && moveVector.x < 0.0 || transform.localScale.x < 0.0 && moveVector.x > 0.0) {
                transform.localScale = transform.localScale.FlippedHorizontally();
            }
            if (moveVector != Vector2.zero) {
                player.state = (CharacterState) 1 + (DeterminePlayerSprinting() > 0.0f ? 1 : 0);
                animator.SetInteger(Constants.AnimationKeys.MoveSpeed, (int) player.state);
            } else {
                player.state = CharacterState.Idle;
                animator.SetInteger(Constants.AnimationKeys.MoveSpeed, (int) player.state);
            }
        }
    }

    private void OnAttackPressed(InputAction.CallbackContext context, int attackIndex)
    {
        InitiateAttack(attackIndex);
    }

    private void OnTestButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("[PC] Test Button Pressed.");
    }

    private void InitiateAttack(int attackIndex) {
        player.InitiateAttack(attackIndex);
    }

    private void OnEnable()
    {
        actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).Enable();
    }

    private void OnDisable()
    {
        actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInputActionMapName).Disable();
    }

    public void EnterHitStun(float hitStunFrames)
    {
        throw new NotImplementedException();
    }

    private float DeterminePlayerSprinting() {
        float isSprinting = Mathf.Abs(sprintAction.ReadValue<float>()) > 0.0f ? 1 : 0;
        if (gamepad != null && SystemInfo.deviceType == DeviceType.Handheld) {
            isSprinting = 
            Mathf.Abs(gamepad.leftStick.ReadValue().x) >= PlayerControllerConstants.joystickSprintThreshold ||
            Mathf.Abs(gamepad.leftStick.ReadValue().y) >= PlayerControllerConstants.joystickSprintThreshold
            ? 1.0f : 0.0f;
        }
        #if UNITY_EDITOR
        if (gamepad != null && isSprinting == 0.0f) {
            isSprinting = 
            Mathf.Abs(gamepad.leftStick.ReadValue().x) >= PlayerControllerConstants.joystickSprintThreshold ||
            Mathf.Abs(gamepad.leftStick.ReadValue().y) >= PlayerControllerConstants.joystickSprintThreshold
            ? 1.0f : 0.0f;
        }
        #endif
        return isSprinting;
    }
}

public enum PlayerControllerState {
    Active,
    Inactive
}