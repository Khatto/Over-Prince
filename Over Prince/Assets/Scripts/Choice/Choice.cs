using JetBrains.Annotations;
using UnityEditor.ShaderGraph.Internal;

public struct Choice {
    public string text;
    public float fontSize;
    public Constants.Emotions? emotion;
    public ChoiceType choiceType;

    public Choice(string text, float fontSize, Constants.Emotions? emotion, ChoiceType choiceType = ChoiceType.Simple) {
        this.text = text;
        this.fontSize = fontSize;
        this.emotion = emotion;
        this.choiceType = choiceType;
    }
}

public enum ChoiceType {
    Emotion,
    Simple
}

public static class ChoiceConstants {

    public static float choiceInteractivityDelay = 0.5f;

    public static class FontSize {
        public const float Small = 40.0f;
        public const float Normal = 60.0f;
        public const float Large = 80.0f;
    }
    public const Constants.Emotions DefaultEmotion = Constants.Emotions.Frenzy;

    public static float emotionChoiceSelectionAnimationDuration = 2.0f;
    public static float choiceSelectionDuration = 0.25f;

    public static class Button {
        public static float widthForTwoButtons = 200.0f;
        public static float widthForThreeButtons = 580.0f;
        public static float xPosForTwoButtons = 400.0f;
        public static float xPosForThreeButtons = 620.0f;

        public static float GetWidthForButtons(int numButtons) {
            if (numButtons == 2) {
                return widthForTwoButtons;
            } else if (numButtons == 3) {
                return widthForThreeButtons;
            } else {
                return widthForTwoButtons;
            }
        }

        public static float GetXPosForButtons(int numButtons, bool isLeft) {
            if (numButtons == 2) {
                return isLeft ? -xPosForTwoButtons : xPosForTwoButtons;
            } else if (numButtons == 3) {
                return isLeft ? -xPosForThreeButtons : xPosForThreeButtons;
            } else {
                return xPosForTwoButtons;
            }
        }
        
    }
}