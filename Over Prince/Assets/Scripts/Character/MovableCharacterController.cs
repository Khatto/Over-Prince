using UnityEngine;

public class MovableCharacterController : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    public float moveSpeed = 2f;

    public float hitStunDuration;
    public float hitStunTimer;
}