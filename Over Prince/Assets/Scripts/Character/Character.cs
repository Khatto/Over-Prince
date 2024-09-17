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
        return state != CharacterState.Dying && state != CharacterState.HitStun && state != CharacterState.Invulnerable;
    }

    /// <summary>
    /// Applies a hit to the character, causing hit stun and knockback via means of the (IHurtable)CharacterController.
    /// </summary>
    /// <param name="hit">The hit information.</param>
    public void ApplyHit(Hit hit, Constants.Direction direction) {
        if (CanBeHit()) {
            if (displayLogs) Debug.Log("(ApplyHit) " + transform.name + " was hit! " + Time.time);
            UpdateHP(hit.damage);
            EnterState(CharacterState.HitStun);
            soundManager.PlayHitContactSound(hit.hitContactSound);
            rigidBody.AddForce(CalculateHitVector(hit, direction), ForceMode2D.Impulse);
            hurtableController.EnterHitStun((hit.hitStunFrames / Constants.targetFPS) * hitStunModifier);
        }
    }

    public void UpdateHP(int damage) {
        stats.currentHP -= damage;
        if (stats.currentHP < 0) {
            stats.currentHP = 0;
        }
        if (displayLogs) Debug.Log(transform.name + " took " + damage + " damage and now has " + stats.currentHP + "/" + stats.maxHP + " HP.");
        hpBar.ChangeHP(-damage);
        if (stats.currentHP <= 0) {
            EnterState(CharacterState.Dying);
        }
        
    }

    public Vector2 CalculateHitVector(Hit hit, Constants.Direction direction) {
        return new Vector2(hit.knockback.x * (int) direction, hit.knockback.y);
    }

    public Constants.Direction GetDirection() {
        return transform.localScale.x >= 0 ? Constants.Direction.Right : Constants.Direction.Left;
    }
}