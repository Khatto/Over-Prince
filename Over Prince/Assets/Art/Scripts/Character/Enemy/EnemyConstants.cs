/// <summary>
/// Details all the potential data values for Enemies based on their EnemyID.
/// </summary>
public static class EnemyConstants {
    internal static class MoveSpeed {
        internal static float TestEnemy = 1.0f;
        internal static float Inadequaseal = 1.0f;
    }

    public static float GetMoveSpeedForEnemy(EnemyID enemyID) {
        return enemyID switch {
            EnemyID.TestEnemy => MoveSpeed.TestEnemy,
            EnemyID.Inadequaseal => MoveSpeed.Inadequaseal,
            _ => 0
        };
    }
}

public enum EnemyID {
    TestEnemy = 0,
    Inadequaseal = 1,
}