/// <summary>
/// Details all the potential data values for the Player as they progress through the game.
/// </summary>
public static class PlayerConstants {
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