using UnityEngine;

/// <summary>
/// Hosts the various global and reusable Constants values in the game
/// </summary>
public static class Constants {

    public static class AnimationKeys {
        public const string MoveSpeed = "moveSpeed";
        public const string PerformAttack = "performAttack";
        public const string ContinueAttack = "continueAttack";
        public const string AttackDesignation = "attackDesignation";
    }

    public static class Colors {
        public static Color transparent = new(0, 0, 0, 0f);
    }

    public static float targetFPS = 60f;

    /// <summary>
    /// The modifier for all character's movement speed when moving vertically.  
    /// This is to account for the nature of Beat 'em Ups where vertical movement sort of indicates going into/out of the plane of the screen.
    /// </summary>
    public static float verticalMovementModifier = 0.65f;

}