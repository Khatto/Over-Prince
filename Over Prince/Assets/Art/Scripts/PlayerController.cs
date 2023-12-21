using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
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

    private Rigidbody2D rigidBody;
    public Animator animator;

    public float moveSpeed = 2f;

    private PlayerConstants.MovementState movementState = PlayerConstants.MovementState.Idle;

    void Start()
    {
        moveAction = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Move);
        sprintAction = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Sprint);

        attack1Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Attack1);
        attack1Action.performed += OnAttack1Pressed;

        attack2Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Attack2);
        attack2Action.performed += OnAttack2Pressed;

        attack3Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Attack3);
        attack3Action.performed += OnAttack3Pressed;

        attack4Action = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.Attack4);
        attack4Action.performed += OnAttack4Pressed;
        
        testAction = actions.FindActionMap(PlayerControllerConstants.InputKeyNames.PlayerInput).FindAction(PlayerControllerConstants.InputKeyNames.TestAction);
        testAction.performed += OnTestButtonPressed;

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    void FixedUpdate()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        UpdateSpriteFromMovement(moveVector);
        if (moveVector != Vector2.zero) {
            rigidBody.MovePosition(rigidBody.position + moveVector * (testMoveSpeed ? moveSpeed : PlayerConstants.GetMoveSpeed(movementState)) * Time.fixedDeltaTime);
        }
    }

    void UpdateSpriteFromMovement(Vector2 moveVector) {
        // TODO: (Performance) Check if it's more performant to only transform if we've changed our moveVector speed, or every method call
        if (transform.localScale.x > 0.0 && moveVector.x < 0.0 || transform.localScale.x < 0.0 && moveVector.x > 0.0) {
            transform.localScale = transform.localScale.FlippedHorizontally();
        }
        if (moveVector != Vector2.zero) {
            float isSprinting = sprintAction.ReadValue<float>();
            movementState = (PlayerConstants.MovementState) 1 + (Mathf.Abs(isSprinting) > 0.0f ? 1 : 0);
        } else {
            movementState = PlayerConstants.MovementState.Idle;
        }
        animator.SetInteger(Constants.AnimationKeys.MoveSpeed, (int) movementState);
    }

    private void OnAttack1Pressed(InputAction.CallbackContext context)
    {
        InitiateAttack(0);
    }

    private void OnAttack2Pressed(InputAction.CallbackContext context)
    {
        InitiateAttack(1);
    }

    private void OnAttack3Pressed(InputAction.CallbackContext context)
    {
        InitiateAttack(2);
    }

    private void OnAttack4Pressed(InputAction.CallbackContext context)
    {
        InitiateAttack(3);
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
}
