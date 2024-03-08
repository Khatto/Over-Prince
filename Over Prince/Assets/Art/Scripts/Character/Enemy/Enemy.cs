using UnityEngine;

public class Enemy : Character
{

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
    }
}