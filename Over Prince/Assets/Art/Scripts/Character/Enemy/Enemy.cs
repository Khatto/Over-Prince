using UnityEngine;

public class Enemy : Character
{
    public EnemyID enemyID = EnemyID.TestEnemy;

    public override void Start()
    {
        SetupAttacks();
        SetupHP();
    }

    public void SetupAttacks() {
        base.Start();
        animator = GetComponent<Animator>();
        attackManager = GetComponent<AttackManager>();
        attacks = EnemyConstants.GetAttacksForEnemy(enemyID);
    }

    public void SetupHP() {
        if (!EnemyConstants.IsBoss(enemyID)) {
            hpBar = GetComponentInChildren<HPBar>();
        }
        stats = EnemyConstants.GetStatsForEnemy(enemyID);
        hpBar.Setup(stats.maxHP);
    }

    public override void EnterState(CharacterState state)
    {
        base.EnterState(state);
        Debug.Log(transform.gameObject.name + " is entering State: " + state);
        switch (state)
        {
            case CharacterState.Idle:
                spriteRenderer.color = Color.white;
                break;
            case CharacterState.Attacking:
                break;
            case CharacterState.HitStun:
                spriteRenderer.color = Constants.Colors.hurtRed;
                break;
            case CharacterState.Dying:
                PerformDeathAnimation();
                break;
            case CharacterState.Invulnerable:
                break;
            case CharacterState.Dead:
                break;
        }
    }

    public void DetermineAndPerformAttack() {
        if (state != CharacterState.Attacking && state != CharacterState.HitStun) {
            Debug.Log(transform.gameObject.name + " is Determining and Performing an attack ");
            int attackIndex = Random.Range(0, attacks.Length);
            InitiateAttack(attackIndex);
        }
    }

    public void InitiateAttack(int attackIndex) {
        if (state != CharacterState.Attacking && state != CharacterState.HitStun && attackIndex < attacks.Length) {
            Debug.Log(transform.gameObject.name + " is Initiating attack " + attacks[attackIndex].attackID);
            EnterState(CharacterState.Attacking);
            animator.SetTrigger(Constants.AnimationKeys.PerformAttack);
            animator.SetInteger(Constants.AnimationKeys.AttackDesignation, (int) attacks[attackIndex].attackID);
            attackManager.PerformAttack(attacks[attackIndex].attackID, HitboxOwner.Enemy, GetDirection());
        }
    }

    public void PerformDeathAnimation() {
        animator.SetTrigger(Constants.AnimationKeys.DeathAnimation);
        hpBar.FadeOut();
        var fade = gameObject.GetComponent<Fade>();
        if (fade != null) {
            fade.StartFadeWithTime(FadeType.FadeOut, Constants.deathFadeTime, () => Die());
        }
    }

    public void Die() {
        EnterState(CharacterState.Dead);
        Destroy(gameObject);
    }

}