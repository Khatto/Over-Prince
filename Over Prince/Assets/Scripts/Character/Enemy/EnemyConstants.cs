using UnityEngine;

/// <summary>
/// Details all the potential data values for Enemies based on their EnemyID.
/// </summary>
public static class EnemyConstants {
    internal static class MoveSpeed {
        internal static float TestEnemy = 1.0f;
        internal static float Inadequaseal = 1.0f;
        internal static float TriangleSlime = 1.0f;
    }

    internal static class Scale {
        internal static Vector2 TestEnemy = new Vector2(0.3f, 0.3f);
        internal static Vector2 TriangleSlime = new Vector2(0.3f, 0.3f);
    }

    public static float GetMoveSpeedForEnemy(EnemyID enemyID) {
        return enemyID switch {
            EnemyID.TestEnemy => MoveSpeed.TestEnemy,
            EnemyID.TriangleSlime => MoveSpeed.TriangleSlime,
            _ => 0
        };
    }

    public static Vector2 GetScaleForEnemy(EnemyID enemyID) {
        return enemyID switch {
            EnemyID.TestEnemy => Scale.TestEnemy,
            EnemyID.TriangleSlime => Scale.TriangleSlime,
            _ => new Vector2(0, 0)
        };
    }

    public static Attack[] GetAttacksForEnemy(EnemyID enemyID) {
        return enemyID switch {
            EnemyID.TestEnemy => new Attack[] { AttackData.GetAttackByAttackID(AttackID.Jab) },
            EnemyID.TriangleSlime => new Attack[] { AttackData.GetAttackByAttackID(AttackID.TriangleSlimeSlam) },
            _ => new Attack[] { }
        };
    }

    public static CharacterStats GetStatsForEnemy(EnemyID enemyID) {
        return enemyID switch {
            EnemyID.TestEnemy => new CharacterStats(1, 0, 0),
            EnemyID.TriangleSlime => new CharacterStats(20, 0, 0),
            _ => new CharacterStats(1, 0, 0),
        };
    }

    public static bool IsBoss(EnemyID enemyID) {
        return enemyID switch {
            EnemyID.TestEnemy => false,
            EnemyID.TriangleSlime => false,
            _ => false
        };
    }
}

public enum EnemyID {
    TestEnemy = 0,
    TriangleSlime = 1
}