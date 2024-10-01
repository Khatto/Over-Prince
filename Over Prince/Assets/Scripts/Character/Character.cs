using UnityEngine;

/// <summary>
/// Represents a character that can may be controlled by a player or AI.  They may attack, or may not.
/// </summary>
public class Character : MonoBehaviour {

    public Animator animator;
    public SpriteRenderer spriteRenderer; // Is this too much responsibility for the Character class?
    public Rigidbody2D rigidBody;
    public CharacterState state = CharacterState.Idle;
    public AttackManager attackManager;
    public MovableCharacterController controller;
    public IHurtableCharacterController hurtableController;
    public Attack[] attacks = new Attack[] {};
    public HPBar hpBar;
    public CharacterStats stats = new CharacterStats(1, 0, 0);
    public CharacterSoundManager soundManager;
    public float hitStunModifier = 1.0f;
    public bool displayLogs = false;

    public virtual void Start() {
        hurtableController = GetComponent<IHurtableCharacterController>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetupSounds();
    }

    public void SetupSounds() {
        soundManager = GetComponentInChildren<CharacterSoundManager>();
    }

    public virtual void EnterState(CharacterState state) {
        this.state = state;
    }

    public virtual void EnterStateFromAnimationStateChange(CharacterState state){
        if (state == CharacterState.Idle && this.state != CharacterState.Disengaged) {
            EnterState(state);
        }
    }

    public bool CanBeHit() {
        return state != CharacterState.Dying && state != CharacterState.Dead && state != CharacterState.Invulnerable;
    }

    /// <summary>
    /// Applies a hit to the character, causing hit stun and knockback via means of the (IHurtable)CharacterController.
    /// </summary>
    /// <param name="hit">The hit information.</param>
    public void ApplyHit(Hit hit, Constants.Direction direction) {
        if (CanBeHit()) {
            if (displayLogs) Debug.Log("(HitStream) " + transform.name + " was hit for damage: " + hit.damage + " at time: " + Time.time);
            soundManager.PlayHitContactSound(hit.hitContactSound);
            UpdateHP(hit.damage);
            EnterState(CharacterState.HitStun);
            rigidBody.AddForce(CalculateHitVector(hit, direction), ForceMode2D.Impulse);
            hurtableController.EnterHitStun((hit.hitStunFrames / Constants.targetFPS) * hitStunModifier);
        }
    }

    public virtual void UpdateHP(int damage) {
        stats.currentHP -= damage;
        if (stats.currentHP < 0) stats.currentHP = 0;
        hpBar.ChangeHP(-damage);
        if (stats.currentHP <= 0) EnterState(CharacterState.Dying);
    }

    public Vector2 CalculateHitVector(Hit hit, Constants.Direction direction) {
        return new Vector2(hit.knockback.x * (int) direction, hit.knockback.y);
    }

    public Constants.Direction GetDirection() {
        return transform.localScale.x >= 0 ? Constants.Direction.Right : Constants.Direction.Left;
    }
}