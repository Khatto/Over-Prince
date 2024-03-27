using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterController, IHurtableCharacterController
{

    public bool testMoveSpeed = false;

    internal static class PlayerControllerConstants {
        internal static class InputKeyNames {
            internal const string PlayerInput = "playerInput";
            internal const string Move = "move";
            internal const string Sprint = "sprint";
            internal const string Attack1 = "attack1";
            internal const string Attack2 = "attack2";
            internal const string Attack3 = "attack3";
            internal const string Attack4 = "attack4";
            internal const string TestAction = "testAction";
        }
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

    void Start()
    {
        SetupActions();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    void FixedUpdate()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        moveVector.y *= Constants.verticalMovementModifier;
        UpdateSpriteFromMovement(moveVector);
        if (moveVector != Vector2.zero && player.state != CharacterState.Attacking) {
            rigidBody.MovePosition(rigidBody.position + moveVector * (testMoveSpeed ? moveSpeed : PlayerConstants.GetMoveSpeed(player.state)) * Time.fixedDeltaTime);
        }
    }

    void SetupActions() {
        moveAction = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Move);
        sprintAction = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Sprint);

        attack1Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Attack1);
        attack1Action.performed += (context) => OnAttackPressed(context, 0);

        attack2Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Attack2);
        attack2Action.performed += (context) => OnAttackPressed(context, 1);

        attack3Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Attack3);
        attack3Action.performed += (context) => OnAttackPressed(context, 2);

        attack4Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Attack4);
        attack4Action.performed += (context) => OnAttackPressed(context, 3);
        
        testAction = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.TestAction);
        testAction.performed += OnTestButtonPressed;
    }

    void UpdateSpriteFromMovement(Vector2 moveVector) {
        // TODO: (Performance) Check if it's more performant to only transform if we've changed our moveVector speed, or every method call
        if (transform.localScale.x > 0.0 && moveVector.x < 0.0 || transform.localScale.x < 0.0 && moveVector.x > 0.0) {
            transform.localScale = transform.localScale.FlippedHorizontally();
        }
        if (player.state != CharacterState.Attacking) {
            if (moveVector != Vector2.zero) {
                float isSprinting = sprintAction.ReadValue<float>();
                player.state = (CharacterState) 1 + (Mathf.Abs(isSprinting) > 0.0f ? 1 : 0);
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
        actions.FindActionMap("playerInput").Enable();
    }

    private void OnDisable()
    {
        actions.FindActionMap("playerInput").Disable();
    }

    public void EnterHitStun(float hitStunFrames)
    {
        throw new NotImplementedException();
    }
}
