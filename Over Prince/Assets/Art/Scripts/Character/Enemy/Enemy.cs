using UnityEngine;

public class Enemy : Character
{
    public EnemyID enemyID = EnemyID.TestEnemy;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
    }
}