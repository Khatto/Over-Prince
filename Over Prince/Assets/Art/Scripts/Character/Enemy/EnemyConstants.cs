/// <summary>
/// Details all the potential data values for Enemies based on their EnemyID.
/// </summary>
public static class EnemyConstants {
    internal static class MoveSpeed {
        internal static float Idle = 0.0f;
        internal static float Walking = 1.75f;
        internal static float Running = 3.75f;
    }

    public static float GetMoveSpeed(CharacterState characterState) {
        return characterState switch {
            CharacterState.Idle => MoveSpeed.Idle,
            CharacterState.Walking => MoveSpeed.Walking,
            CharacterState.Running => MoveSpeed.Running,
            _ => 0
        };
    }
}

public enum EnemyID {
    TestEnemy = 0,
    Ranged
}