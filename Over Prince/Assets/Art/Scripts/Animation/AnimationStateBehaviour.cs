using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateBehaviour : StateMachineBehaviour
{

    [SerializeField] private CharacterState stateToTransitionToOnEnter;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<Player>().EnterState(stateToTransitionToOnEnter);
    }
}
