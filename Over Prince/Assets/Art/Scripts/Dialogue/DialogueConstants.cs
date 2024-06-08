public static class DialogueConstants {

    public static class CommonFormattedTerms {
        public static string TheField = $"<color={Colors.redColor}>The Field</color>";
        public static string TheFile = $"<color={Colors.redColor}>The File</color>";
        public static string End = $"<color={Colors.redColor}>END</color>";
        public static string Elevators = $"<color={Colors.elevatorColor}>Elevators</color>";
    }

    public static float TotalDialogueTime(Dialogue[] dialogues, float dialogueFadeTime) {
        float total = 0.0f;
        foreach (Dialogue dialogue in dialogues) {
            total += dialogue.displayTime + dialogueFadeTime * 2.0f;
        }
        return total;
    }

    public static class IntroScene {
        public static class PartOne {
            public static Dialogue[] dialogues = {
                new Dialogue("I don't remember how long I've been stuck in this place...", "Music/Song_Name", 4),
                new Dialogue("Whatever... Wherever... \"this place\" even is...", "Music/Song_Name", 4)
            };
        }

        public static class PartFour {
            public static Dialogue[] dialogues = {
                new Dialogue($"Some people call this place {CommonFormattedTerms.TheField}...", "Music/Song_Name", 3),
                new Dialogue("Because no matter how far you feel you get,", "Music/Song_Name", 4),
                new Dialogue("It just keeps stretching on...", "Music/Song_Name", 4),
            };
        }

        public static class PartSix {
            public static Dialogue[] dialogues = {
                new Dialogue($"Some call it {CommonFormattedTerms.TheFile}.", "Music/Song_Name", 3),
                new Dialogue("Because at times it feels like we're programs in some simulation...", "Music/Song_Name", 4),
                new Dialogue("Maybe someone's watching us and collecting data or something...", "Music/Song_Name", 3.5f),
            };
        }

        public static class PartEight {
            public static Dialogue[] dialogues = {
                new Dialogue("Whatever you wanna call this place, I can't recall how many times...", "Music/Song_Name", 4),
                new Dialogue($"I've got on one of these {CommonFormattedTerms.Elevators}...", "Music/Song_Name", 4),
            };
        }

        public static class PartTen {
            public static Dialogue[] dialogues = {
                new Dialogue("Fighting my way through different Stages...", "Music/Song_Name", 3),
                new Dialogue("Thinking that I'm getting somewhere...", "Music/Song_Name", 3),
            };
        }

        public static class PartTwelve {
            public static Dialogue[] dialogues = {
                new Dialogue("Just to end up back here...", "Music/Song_Name", 3),
                new Dialogue(" ", "Music/Song_Name", 1),
                new Dialogue("I'm so tired of this cycle...", "Music/Song_Name", 3),
                new Dialogue("I literally can't take this anymore...", "Music/Song_Name", 3),
                new Dialogue("All I know is...", "Music/Song_Name", 3),
                new Dialogue(" ", "Music/Song_Name", 1),
            };
        }

        public static class PartThirteen {
            public static Dialogue[] dialogues = {
                new Dialogue($"This all has to {CommonFormattedTerms.End}!", "Music/Song_Name", 5)
            };
        }
    }

}