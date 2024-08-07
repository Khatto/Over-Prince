using UnityEngine;

public class Player : Character
{
    public Attack[] attacks = new Attack[] {
        AttackData.GetAttackByAttackID(AttackID.Jab),
    };

    public override void Start()
    {
        Setup();
    }

    void Setup() {
        base.Start();
        animator = GetComponent<Animator>();
        attackManager = GetComponent<AttackManager>();
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

}