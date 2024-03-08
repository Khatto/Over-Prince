using UnityEngine;

/// <summary>
/// Represents a character that can may be controlled by a player or AI.  They may attack, or may not.
/// </summary>
public class Character : MonoBehaviour {

    public Animator animator;
    public Rigidbody2D rigidBody;

    public CharacterState state = CharacterState.Idle;

    public AttackManager attackManager;

    public void CheckForAttackCollisions() {
        
    }

    public void EnterState(CharacterState state) {
        this.state = state;
    }

}