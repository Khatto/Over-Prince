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

}