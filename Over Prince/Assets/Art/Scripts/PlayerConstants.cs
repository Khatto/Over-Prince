/// <summary>
/// Details all the potential data values for the Player as they progress through the game.
/// </summary>
public static class PlayerConstants {
    internal static class MoveSpeed {
        internal static float Idle = 0.0f;
        internal static float Walking = 1.75f;
        internal static float Running = 3.75f;
    }

    public enum MovementState {
        Idle = 0,
        Walking = 1,
        Running = 2
    }

    public static float GetMoveSpeed(MovementState movementState) {
        return movementState switch {
            MovementState.Idle => MoveSpeed.Idle,
            MovementState.Walking => MoveSpeed.Walking,
            MovementState.Running => MoveSpeed.Running,
            _ => 0
        };
    }
}