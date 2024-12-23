using UnityEngine;

public static class DialogueConstants {

    public static class CommonFormattedTerms {
        public static string TheField = $"<color={Colors.redColor}>The Field</color>";
        public static string TheFile = $"<color={Colors.redColor}>The File</color>";
        public static string End = $"<color={Colors.redColor}>END</color>";
        public static string Elevators = $"<color={Colors.elevatorColor}>Elevators</color>";
        public static string Elevator = $"<color={Colors.elevatorColor}>Elevator</color>";
        public static string QuestionMarkDialogueName = $"<color={Colors.dialogueNameColor}>???:</color>";
    }

    public static class TestDialogue {
            public static Dialogue[] dialogues = {
                new Dialogue($"Test!", "Music/Song_Name", 0.0f)
            };

            public static Dialogue[] doubleChoiceTest = {
                new Dialogue(
                    "Double Choice Test", 
                    "Music/Song_Name",
                    0,
                    new Choice[] {
                        new Choice("Choice 1", ChoiceConstants.FontSize.Normal, Constants.Emotions.Frenzy),
                        new Choice("Choice 2", ChoiceConstants.FontSize.Normal, Constants.Emotions.Confusion)
                    },
                    new DialogueTrait[] { DialogueTrait.StopSpeaking }
                )
            };

            public static Dialogue[] tripleChoiceTest = {
                new Dialogue(
                    "Triple Choice Test", 
                    "Music/Song_Name",
                    0,
                    new Choice[] {
                        new Choice("Choice 1", ChoiceConstants.FontSize.Normal, Constants.Emotions.Frenzy),
                        new Choice("Choice 2", ChoiceConstants.FontSize.Normal, Constants.Emotions.Confusion),
                        new Choice("Choice 3", ChoiceConstants.FontSize.Normal, Constants.Emotions.Sorrow)
                    },
                    new DialogueTrait[] { DialogueTrait.StopSpeaking }
                )
            };
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

    public static class FieldLobbyIntro {
        public static class PartOne {
            public static Dialogue[] dialogues = {
                new Dialogue("Waking up back here... Again...", "Music/Song_Name", 0),
                new Dialogue($"Should I try another {CommonFormattedTerms.Elevator}?", "Music/Song_Name", 0),
                new Dialogue("For what though? When I'll probably just end up back here, AGAIN...", "Music/Song_Name", 0),
                new Dialogue("But what else is there to do?", "Music/Song_Name", 0),
            };
        }

        public static class BattleIntroDialoguePartOne {
            public static Dialogue[] dialogues = {
                new Dialogue("Wait... I see a Monster!", "Music/Song_Name", 0),
            };
        }

        public static class BattleIntroDialoguePartTwo {
            public static Dialogue[] dialogues = {
                new Dialogue("They're dangerous, but it looks off guard...", "Music/Song_Name", 0),
                new Dialogue("I should sneak up and take it out!", "Music/Song_Name", 0),
            };
        }

        public static class BattlePostEnemyHit {
            public static Dialogue[] dialogues = {
                new Dialogue("Uh oh...  It's angry now.", "Music/Song_Name", 0),
                new Dialogue("I need to take it out before it takes ME out!", "Music/Song_Name", 0),
            };
        }

        public static class BattleComplete {
            public static Dialogue[] dialogues = {
                new Dialogue("Whew...  That was close.", "Music/Song_Name", 0),
                new Dialogue("Let me keep looking around...", "Music/Song_Name", 0),
            };
        }

        public static class HoodedBoyEncounter {
            public static Dialogue[] dialogues = {
                new Dialogue($"{CommonFormattedTerms.QuestionMarkDialogueName} Can you feel that? ...", "Music/Song_Name", 0),
                new Dialogue(
                    "Can you feel that?", 
                    "Music/Song_Name",
                    0,
                    new Choice[] {
                        new Choice("And who are you?", ChoiceConstants.FontSize.Normal, Constants.Emotions.Frenzy, ChoiceType.Emotion),
                        new Choice("Yeah I do...", ChoiceConstants.FontSize.Normal, Constants.Emotions.Confusion, ChoiceType.Emotion),
                        new Choice("Huh? Feel what?", ChoiceConstants.FontSize.Normal, Constants.Emotions.Sorrow, ChoiceType.Emotion),
                    },
                    new DialogueTrait[] { DialogueTrait.StopSpeaking }
                ),
                new Dialogue(
                    $"{CommonFormattedTerms.QuestionMarkDialogueName} Things are changing...",
                    "Music/Song_Name",
                    0,
                    null,
                    new DialogueTrait[] { DialogueTrait.StartSpeaking }
                ),
                new Dialogue($"{CommonFormattedTerms.QuestionMarkDialogueName} EVERYTHING, in fact... is changing...", "Music/Song_Name", 0),
            };
        }

        public static class HoodedBoyFinalMessage {
            public static Dialogue[] dialogues = {
                new Dialogue(
                    "???: Change can be positive or negative.",
                    "Music/Song_Name", 
                    0,
                    null,
                    new DialogueTrait[] { DialogueTrait.StartSpeaking }
                ),
                new Dialogue("???: It can take you up, or down...", "Music/Song_Name", 0),
                new Dialogue("???: If change is what you seek,", "Music/Song_Name", 0),
                new Dialogue($"???: Take this {CommonFormattedTerms.Elevator}.", "Music/Song_Name", 0)
            };
        }

        public static class ElevatorConfirmation {
            public static Dialogue[] dialogues = {
                new Dialogue(
                    "Take the Elevator?", 
                    "Music/Song_Name",
                    0,
                    new Choice[] {
                        new Choice(SimpleChoices.Yes, ChoiceConstants.FontSize.Normal, null),
                        new Choice(SimpleChoices.No, ChoiceConstants.FontSize.Normal, null)
                    },
                    new DialogueTrait[] { DialogueTrait.StopSpeaking }
                )
            };
        }

        public static class HoodedBoyReassurance {

            public static Dialogue reassurance1 = new Dialogue(
                "???: Don't worry, you'll be fine...",
                "Music/Song_Name", 
                0,
                null,
                new DialogueTrait[] { DialogueTrait.StartSpeaking }
            );

            public static Dialogue reassurance2 = new Dialogue(
                "???: Don't be afraid to embrace change...",
                "Music/Song_Name", 
                0,
                null,
                new DialogueTrait[] { DialogueTrait.StartSpeaking }
            );

            public static Dialogue reassurance3 = new Dialogue(
                "???: You can do this...",
                "Music/Song_Name", 
                0,
                null,
                new DialogueTrait[] { DialogueTrait.StartSpeaking }
            );

            public static Dialogue reassurance4 = new Dialogue(
                "???: Your future is in your hands...",
                "Music/Song_Name", 
                0,
                null,
                new DialogueTrait[] { DialogueTrait.StartSpeaking }
            );

            public static Dialogue reassurance5 = new Dialogue(
                "???: Don't worry, you'll be fine.",
                "Music/Song_Name", 
                0,
                null,
                new DialogueTrait[] { DialogueTrait.StartSpeaking }
            );
            
            public static Dialogue reassuranceFailure = new Dialogue(
                "???: Your fear of change will be your end.",
                "Music/Song_Name", 
                0,
                null,
                new DialogueTrait[] { DialogueTrait.StartSpeaking }
            );

            public static Dialogue[] RandomDialogue() {
                // Return a random dialogue from the various reassurance dialogues in a Dialogue array
                Dialogue[] dialogues = {
                    reassurance1,
                    reassurance2,
                    reassurance3,
                    reassurance4,
                    reassurance5
                };
                return new Dialogue[] { dialogues[Random.Range(0, dialogues.Length)] };

            }
        }
    }

} 