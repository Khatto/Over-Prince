public static class DialogueConstants {

    public static class IntroScene {
        public static class PartOne {

            public const string test = "Test";

            public static Dialogue[] dialogues = {
                new Dialogue("At this point... I forgot how long I've been stuck in this place...", "Music/Song_Name"),
                new Dialogue("Stuck here... Just floating by...", "Music/Song_Name")
            };
        }

        public static class PartTwo {
            public static Dialogue[] dialogues = {
                new Dialogue("Some people call this place The Field...", "Music/Song_Name"),
                new Dialogue("Because no matter how far you feel you get,", "Music/Song_Name"),
                new Dialogue("It just keeps stretching on...", "Music/Song_Name"),
            };
        }

        public static class PartThree {
            public static Dialogue[] dialogues = {
                new Dialogue("Some call it The File.", "Music/Song_Name"),
                new Dialogue("Because maybe we're just programs in some computer...", "Music/Song_Name"),
                new Dialogue("And our whole existence is being watched data collection by somebody out there...", "Music/Song_Name"),
            };
        }
    }

}