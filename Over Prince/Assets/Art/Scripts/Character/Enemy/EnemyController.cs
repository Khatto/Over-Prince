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
    private float hitStunTimer;
    private float hitStunDuration;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        targetCollider = target.GetComponent<Collider2D>();
        moveSpeed = EnemyConstants.GetMoveSpeedForEnemy(GetComponent<Enemy>().enemyID);
        enemy = GetComponent<Enemy>();
    }

    void FixedUpdate()
    {
        if (enemy.state != CharacterState.HitStun && enemy.state != CharacterState.Dead) {
            LinearlyMoveTowardsTarget();
        }
        if (enemy.state == CharacterState.HitStun) {
            hitStunTimer += Time.fixedDeltaTime;
            if (hitStunTimer >= hitStunDuration) {
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
        if (!isTouchingTarget && Vector3.Distance(target.transform.position, rigidBody.transform.position) > minDistanceFromTarget)
        {
            Vector3 direction = (target.transform.position - rigidBody.transform.position).normalized;
            Vector3 moveVector = direction * moveSpeed * Time.fixedDeltaTime;
            moveVector.y *= Constants.verticalMovementModifier;
            rigidBody.MovePosition(rigidBody.transform.position + moveVector);
        }
    }

    /// <summary>
    /// Prepares the enemy for entering Hitstun by taking in the hitStunDuration.
    /// </summary>
    /// <param name="hitStunDuration">The duration of the hit stun state.</param>
    public void EnterHitStun(float hitStunDuration)
    {
        this.hitStunDuration = hitStunDuration;
        Debug.Log("Enemy hitstun entered for " + hitStunDuration + " seconds.");
    }
    
}