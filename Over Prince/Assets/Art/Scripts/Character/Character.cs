using UnityEngine;

/// <summary>
/// Represents a character that can may be controlled by a player or AI.  They may attack, or may not.
/// </summary>
public class Character : MonoBehaviour {

    public Animator animator;
    public Rigidbody2D rigidBody;
    public CharacterState state = CharacterState.Idle;
    public AttackManager attackManager;
    public CharacterController controller;
    public IHurtableCharacterController hurtableController;

    public virtual void Start() {
        Debug.Log("Character Start");
        hurtableController = GetComponent<IHurtableCharacterController>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public virtual void EnterState(CharacterState state) {
        this.state = state;
    }

    public bool CanBeHit() {
        return state != CharacterState.Dead && state != CharacterState.HitStun && state != CharacterState.Invulnerable;
    }

    /// <summary>
    /// Applies a hit to the character, causing hit stun and knockback via means of the (IHurtable)CharacterController.
    /// </summary>
    /// <param name="hit">The hit information.</param>
    public void ApplyHit(Hit hit, Constants.Direction direction) {
        if (CanBeHit()) {
            EnterState(CharacterState.HitStun);
            rigidBody.velocity = Vector2.zero;
            rigidBody.AddForce(CalculateHitVector(hit, direction), ForceMode2D.Impulse);
            hurtableController.EnterHitStun(hit.hitStunFrames / Constants.targetFPS);
        }
    }

    public Vector2 CalculateHitVector(Hit hit, Constants.Direction direction) {
        return new Vector2(hit.knockback.x * (int) direction, hit.knockback.y);
    }

    public Constants.Direction GetDirection() {
        return transform.localScale.x >= 0 ? Constants.Direction.Right : Constants.Direction.Left;
    }

}