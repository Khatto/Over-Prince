using UnityEngine;

/// <summary>
/// Hosts the various global and reusable Constants values in the game
/// </summary>
public static class Constants {

    public static class Scenes {
        public const string Title = "TitleScreen";
        public const string Intro = "IntroScene";
        public const string Playground = "PlaygroundScene";
        public const string SceneOne = "SceneOne";
        public const float SceneTransitionTime = .65f;
    }

    public static class TagKeys {
        public const string Hurtbox = "Hurtbox";
        public const string Hitbox = "Hitbox";
    }

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

    public enum Direction {
        Left = -1,
        Right = 1
    }

    public enum Song {
        Juxtaposition,
        TheEnd
    }

    public static class SongDatas {
        public static SongData juxtaposition = new SongData("Juxtaposition", "Khiry Arnold");
        public static SongData theEnd = new SongData("The End", "Khiry Arnold");

        public static SongData GetSongData(Song song) {
            return song switch {
                Song.Juxtaposition => juxtaposition,
                Song.TheEnd => theEnd,
                _ => juxtaposition
            };
        }
    }
}