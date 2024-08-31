using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Controls the behavior of an enemy character, primarily the movement.
/// </summary>
public class EnemyController : MovableCharacterController, IHurtableCharacterController {

    private Collider2D collider2D;
    public Transform target;
    public Collider2D targetCollider;
    public float minDistanceFromTarget = 0.09f;
    private Enemy enemy;
    private Vector2 defaultScale;
    private float hitStunTimer;
    private float hitStunDuration;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        targetCollider = target.GetComponent<Collider2D>();
        enemy = GetComponent<Enemy>();
        moveSpeed = EnemyConstants.GetMoveSpeedForEnemy(enemy.enemyID);
        defaultScale = EnemyConstants.GetScaleForEnemy(enemy.enemyID);
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        transform.localScale = EnemyConstants.GetScaleForEnemy(EnemyID.TriangleSlime);
    }

    void FixedUpdate()
    {
        if (enemy.state != CharacterState.HitStun && enemy.state != CharacterState.Dying) {
            LinearlyMoveTowardsTarget();
        }
        if (enemy.state == CharacterState.HitStun) {
            hitStunTimer += Time.fixedDeltaTime;
            if (hitStunTimer >= hitStunDuration) {
                animator.SetTrigger(Constants.AnimationKeys.RecoverFromHurt);
                enemy.EnterState(CharacterState.Idle);
                hitStunTimer = 0;
            }
        }
    }

    /// <summary>
    /// Moves the enemy character linearly towards the target.
    /// </summary>
    void LinearlyMoveTowardsTarget()
    {   
        bool isTouchingTarget = collider2D.IsTouching(targetCollider);
        if (!isTouchingTarget && !WithinTargetRange() && enemy.state != CharacterState.HitStun)
        {
            Vector3 direction = (target.transform.position - rigidBody.transform.position).normalized;
            Vector3 moveVector = direction * moveSpeed * Time.fixedDeltaTime;
            moveVector.y *= Constants.verticalMovementModifier;
            rigidBody.MovePosition(rigidBody.transform.position + moveVector);
            if (moveVector.x > 0)
            {
                rigidBody.transform.localScale = new Vector3(defaultScale.x, defaultScale.y, 1);
            }
            else if (moveVector.x < 0)
            {
                rigidBody.transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, 1);
            }
            animator.SetFloat("moveSpeed", moveVector.magnitude);
        } else {
            animator.SetFloat("moveSpeed", 0);
        }
        if (WithinTargetRange() || isTouchingTarget)
        {
            enemy.DetermineAndPerformAttack();
        }
    }

    public bool WithinTargetRange()
    {
        return Vector3.Distance(target.transform.position, rigidBody.transform.position) <= minDistanceFromTarget;
    }

    /// <summary>
    /// Prepares the enemy for entering Hitstun by taking in the hitStunDuration.
    /// </summary>
    /// <param name="hitStunDuration">The duration of the hit stun state.</param>
    public void EnterHitStun(float hitStunDuration)
    {
        this.hitStunDuration = hitStunDuration;
        animator.SetTrigger(Constants.AnimationKeys.Hurt);
        enemy.attackManager.DestroyInterruptibleHitboxes();
    }
}