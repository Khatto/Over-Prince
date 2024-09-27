using UnityEngine;

public class Player : Character
{
    public HPPortraitManager hpPortraitManager;
    
    public override void Start()
    {
        Setup();
        SetupAttacks();
        SetupHP();
    }

    void Setup() {
        base.Start();
        animator = GetComponent<Animator>();
        attackManager = GetComponent<AttackManager>();
    }

    void SetupAttacks() {
        attacks = new Attack[] {
            AttackData.GetAttackByAttackID(AttackID.Jab),
        };
    }

    void SetupHP() {
        hpPortraitManager = GameObject.FindGameObjectWithTag(Constants.TagKeys.PlayerHPPortrait).GetComponent<HPPortraitManager>();
        hpBar = GameObject.FindGameObjectWithTag(Constants.TagKeys.PlayerHPBar).GetComponent<HPBar>();
        stats = new CharacterStats(20, 0, 0);
        hpBar.Setup(stats.maxHP);
    }

    public void InitiateAttack(int attackIndex) {
        if (state != CharacterState.Attacking && attackIndex < attacks.Length) {
            state = CharacterState.Attacking;
            animator.SetTrigger(Constants.AnimationKeys.PerformAttack);
            animator.SetInteger(Constants.AnimationKeys.AttackDesignation, (int) attacks[attackIndex].attackID);
            attackManager.PerformAttack(attacks[attackIndex].attackID, HitboxOwner.Player, GetDirection());
        }
        else if (state == CharacterState.Attacking) {
            if (ShouldContinueJabAttack(attackIndex)) {
                animator.SetTrigger(Constants.AnimationKeys.ContinueAttack);
                if (IsPerformingJab2()) {
                    attackManager.PerformAttack(AttackID.Jab3, HitboxOwner.Player, GetDirection());
                } else {
                    attackManager.PerformAttack(AttackID.Jab2, HitboxOwner.Player, GetDirection());
                }
            }
        }
    }

    private bool ShouldContinueJabAttack(int attackIndex) {
        return attackIndex < attacks.Length 
        && attacks[attackIndex].IsJab()
        && !IsPerformingJab3();
    }

    private bool IsPerformingJab2() {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(AttackData.AnimationKeys.Jab2);
    }

    private bool IsPerformingJab3() {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(AttackData.AnimationKeys.Jab3);
    }

    public override void UpdateHP(int damage)
    {
        base.UpdateHP(damage);
        if (damage > 0) hpPortraitManager.PerformPortraitAction(PortraitAction.Hurt);
    }

}