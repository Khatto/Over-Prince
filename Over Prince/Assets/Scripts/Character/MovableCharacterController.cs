using UnityEngine;

public class MovableCharacterController : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    public float moveSpeed = 2f;

    public float hitStunDuration;
    public float hitStunTimer;

    public virtual void PerformUniqueAction(SpecialCharacterAction action) {
        /* Implement in child class */
    }

    public float GetVelocity() {
        return rigidBody.linearVelocityX;
    }
}