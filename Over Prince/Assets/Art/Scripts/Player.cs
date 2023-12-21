using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Attack[] attacks = new Attack[] {
        AttackData.GetAttackByAttackID(AttackID.Jab),
    };
    public Animator animator;
    public AttackManager attackManager;
    public PlayerState state = PlayerState.Idle;

    void Start()
    {
        animator = GetComponent<Animator>();
        attackManager = GetComponent<AttackManager>();
    }

    public void EnterState(PlayerState state) {
        this.state = state;
    }

    public void InitiateAttack(int attackIndex) {
        if (state != PlayerState.Attacking && attackIndex < attacks.Length) {
            state = PlayerState.Attacking;
            animator.SetTrigger(Constants.AnimationKeys.PerformAttack);
            animator.SetInteger(Constants.AnimationKeys.AttackDesignation, (int) attacks[attackIndex].attackID);
            attackManager.PerformAttack(attacks[attackIndex].attackID);
        }
        else if (state == PlayerState.Attacking) {
            if (ShouldContinueJabAttack(attackIndex)) {
                animator.SetTrigger(Constants.AnimationKeys.ContinueAttack);
                if (IsPerformingJab2()) {
                    attackManager.PerformAttack(AttackID.Jab3);
                } else {
                    attackManager.PerformAttack(AttackID.Jab2);
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

public enum PlayerState {
    Idle,
    Walking,
    Running,
    Attacking 
}