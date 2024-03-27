using UnityEngine;

public class Enemy : Character
{
    public EnemyID enemyID = EnemyID.TestEnemy;

    public override void EnterState(CharacterState state)
    {
        base.EnterState(state);
        switch (state)
        {
            case CharacterState.Idle:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case CharacterState.Attacking:
                break;
            case CharacterState.HitStun:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case CharacterState.Dead:
                break;
            case CharacterState.Invulnerable:
                break;
        }
        
    }

}