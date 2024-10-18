public struct Choice {
    public string text;
    public float fontSize;
    public Constants.Emotions emotion;

    public Choice(string text, float fontSize, Constants.Emotions emotion) {
        this.text = text;
        this.fontSize = fontSize;
        this.emotion = emotion;
    }
}

public static class ChoiceConstants {

    public static float choiceInteractivityDelay = 0.5f;

    public static class FontSize {
        public const float Small = 40.0f;
        public const float Normal = 60.0f;
        public const float Large = 80.0f;
    }
    public const Constants.Emotions DefaultEmotion = Constants.Emotions.Frenzy;

    public static float choiceSelectionAnimationDuration = 2.0f;
}