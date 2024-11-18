using UnityEngine;
using System;

/// <summary>
/// Hosts the various global and reusable Constants values in the game
/// </summary>
public static class Constants {

    public enum Emotions { // TODO: Consider moving this to it's own class?
        
        Frenzy,
        Sorrow,
        Confusion
    }

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
        public const string PlayerHPBar = "Player HP Bar";
        public const string PlayerHPPortrait = "Player HP Portrait";
        public const string HitStopManager = "HitStopManager";
        public const string ChoiceFlasher = "Choice Flasher";
    }

    public static class AnimationKeys {
        public const string MoveSpeed = "moveSpeed";
        public const string PerformAttack = "performAttack";
        public const string ContinueAttack = "continueAttack";
        public const string AttackDesignation = "attackDesignation";
        public const string Start = "start";
        public const string Hurt = "hurt";
        public const string RecoverFromHurt = "recoverFromHurt";
        public const string DeathAnimation = "deathAnimation";
        public const string Walking = "walking";
    }

    public static class Colors {
        public static Color transparent = new(0, 0, 0, 0f);

        public static Color hitboxStartUpColor = new(0, 1, 0, 1.0f);

        public static Color hitboxActiveColor = new(1, 0, 0, 1.0f);

        public static Color hitboxCoolDownColor = new(0, 0, 1, 1.0f);

        public static Color hurtRed = new(1.0f, 0.71f, 0.71f, 1.0f);

        public static Color hpBarGreen = new(0.18f, .75f, 0.03f, 1.0f); // #2DBE08

        public static Color hpBarBackground = new(0.0f, 0.0f, 0.0f, 1.0f);

        public static Color frenzyColor = new Color(0.57f, 0.06f, 0.06f, 1.0f); // #921010

        public static Color sorrowColor = new Color(0.062f, 0.063f, 0.573f, 1.0f); // #101092

        public static Color confusionColor = new Color(0.31f, 0.02f, 0.66f, 1.0f); // #4F06A9

        public static Color GetEmotionColor(Emotions emotion) {
            return emotion switch {
                Emotions.Frenzy => frenzyColor,
                Emotions.Sorrow => sorrowColor,
                Emotions.Confusion => confusionColor,
                _ => Color.white
            };
        }

        public static Color GetColorFullyVisible(Color color) {
            return new Color(color.r, color.g, color.b, 1.0f);
        }

        public static Color GetColorFullyTransparent(Color color) {
            return new Color(color.r, color.g, color.b, 0.0f);
        }
    }

    public static float targetFPS = 60f;

    /// <summary>
    /// The modifier for all character's movement speed when moving vertically.  
    /// This is to account for the nature of Beat 'em Ups where vertical movement sort of indicates going into/out of the plane of the screen.
    /// </summary>
    public static float verticalMovementModifier = 0.65f;

    public static float deathFadeTime = 0.5f;

    public enum Direction {
        Left = -1,
        Right = 1
    }

    public enum Song {
        Juxtaposition,
        TheEnd
    }

    public static class SongDatas {
        public static SongData juxtaposition = new SongData("Juxtaposition", "Khiry Alio");
        public static SongData theEnd = new SongData("Spacious", "Khiry Alio");

        public static SongData GetSongData(Song song) {
            return song switch {
                Song.Juxtaposition => juxtaposition,
                Song.TheEnd => theEnd,
                _ => juxtaposition
            };
        }
    }

    public static string GenerateRandomDigits()
    {
        System.Random random = new System.Random();
        string randomDigits = "";

        for (int i = 0; i < 6; i++)
        {
            randomDigits += random.Next(0, 10);
        }

        return randomDigits;
    }

}