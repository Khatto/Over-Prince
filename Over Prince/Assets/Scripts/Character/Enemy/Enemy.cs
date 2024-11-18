using UnityEngine;

public class Enemy : Character
{
    public EnemyID enemyID = EnemyID.TestEnemy;
    public SpecialCharacterType specialBattleType = SpecialCharacterType.None;

    public override void Start()
    {
        base.Start();
        SetupAttacks();
        SetupHP();
        SetupHitStun();
        SetupDeathParticles();
    }

    public void SetupAttacks() {
        base.Start();
        // animator = GetComponent<Animator>();
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

    public void SetupHitStun() {
        hitStunModifier = EnemyConstants.GetHitStunModifierForEnemy(enemyID);
    }

    public void SetToMaxHP() {
        stats.currentHP = stats.maxHP; // TODO: Something about these implementations is a bit redundant
        hpBar.SetToMaxHP();
    }

    public void SetupDeathParticles() {
        deathGenerator = GetComponentInChildren<CommonParticleGenerator>();
    }

    public override void EnterState(CharacterState state)
    {
        if (this.state == CharacterState.Dying && state != CharacterState.Dead) return;
        base.EnterState(state);
        if (displayLogs) Debug.Log("(HitStream) Enemy " + name + " is entering state: " + state + " at time " + Time.time);
        switch (state)
        {
            case CharacterState.Idle:
                if (specialBattleType == SpecialCharacterType.IntroTutorial) {
                    PerformUniqueAction(SpecialCharacterAction.FaceCharacterAfterHitStun);
                }
                spriteRenderer.color = Color.white;
                break;
            case CharacterState.Attacking:
                break;
            case CharacterState.HitStun:
                if (displayLogs) Debug.Log("Enemy is in HitStun at time: " + Time.time);
                attackManager.DestroyInterruptibleHitboxes();
                break;
            case CharacterState.Dying:
                if (displayLogs) Debug.Log("Enemy is in Dying at time: " + Time.time);
                PerformDeathAnimation();
                attackManager.DestroyInterruptibleHitboxes();
                break;
            case CharacterState.Invulnerable:
                break;
            case CharacterState.Dead:
                break;
        }
    }

    public void DetermineAndPerformAttack() {
        if (state != CharacterState.Attacking && state != CharacterState.HitStun) {
            if (displayLogs) Debug.Log(transform.gameObject.name + " is Determining and Performing an attack at time: " + Time.time + " in state: " + state);
            int attackIndex = Random.Range(0, attacks.Length);
            InitiateAttack(attackIndex);
        }
    }

    public void InitiateAttack(int attackIndex) {
        if (state != CharacterState.Attacking && state != CharacterState.HitStun && attackIndex < attacks.Length) {
            if (displayLogs) Debug.Log("(HitStream) " + transform.gameObject.name + " is Initiating attack " + attacks[attackIndex].attackID);
            EnterState(CharacterState.Attacking);
            animator.ResetTrigger(Constants.AnimationKeys.RecoverFromHurt);
            animator.ResetTrigger(Constants.AnimationKeys.Hurt);
            animator.SetTrigger(Constants.AnimationKeys.PerformAttack);
            animator.SetInteger(Constants.AnimationKeys.AttackDesignation, (int) attacks[attackIndex].attackID);
            attackManager.PerformAttack(attacks[attackIndex].attackID, HitboxOwner.Enemy, GetDirection());
        }
    }

    public void PerformDeathAnimation() {
        animator.SetTrigger(Constants.AnimationKeys.DeathAnimation);
        hpBar.FadeHPBar(FadeType.FadeOut);
        soundManager.PlayDeathSound();
        var fade = gameObject.GetComponent<Fade>();
        if (deathGenerator != null) deathGenerator.GenerateParticles(transform.position);
        if (fade != null) fade.StartFadeWithTime(FadeType.FadeOut, Constants.deathFadeTime, () => Die());
    }

    public void Die() {
        EnterState(CharacterState.Dead);
        attackManager.DestroyInterruptibleHitboxes();
        Destroy(gameObject);
    }

    public void PerformUniqueAction(SpecialCharacterAction action) {
        switch (action) {
            case SpecialCharacterAction.FaceCharacterAfterHitStun:
                EnterState(CharacterState.Disengaged);
                GetComponent<EnemyController>().PerformUniqueAction(action);
                break;
        }
    }

    public void StartBattle() {
        if (displayLogs) Debug.Log("Enemy " + name + " is starting battle from state " + state);
        EnterState(CharacterState.Idle);
        hpBar.DisplayHPBar();
    }

    public void StopBattle() {
        if (displayLogs) Debug.Log("Enemy " + name + " is stopping battle from state " + state);
        EnterState(CharacterState.Disengaged);
        hpBar.FadeHPBar(FadeType.FadeOut);
        GetComponentInChildren<HurtboxManager>().state = HurtboxState.Inactive;
    }
}

