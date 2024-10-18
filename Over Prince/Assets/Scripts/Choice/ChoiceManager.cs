using UnityEngine;
using System.Collections;

public class ChoiceManager : MonoBehaviour {
    public static ChoiceManager instance;

    public Choice currentChoice;
    public PlayerStats playerStats;
    public Player player;
    public ChoiceParticleGenerator choiceParticleGenerator;

    public void Awake() {
        SetupSingleton();
        FindPlayer();
    }

    public void SetupSingleton() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void FindPlayer() {
        player = FindFirstObjectByType<Player>();
    }

    public void SetPlayer(Player player) {
        this.player = player;
    }

    public void MakeEmotionChoice(Choice choice, Vector3 position) {
        if (player != null) {
            switch (choice.emotion) {
                case Constants.Emotions.Frenzy:
                    playerStats.totalFrenzyPoints += 1;
                    break;
                case Constants.Emotions.Sorrow:
                    playerStats.totalSorrowPoints += 1;
                    break;
                case Constants.Emotions.Confusion:
                    playerStats.totalConfusionPoints += 1;
                    break;
            }
        }
        StartCoroutine(EmotionChoiceAnimation(choice.emotion, position));
    }

    public IEnumerator EmotionChoiceAnimation(Constants.Emotions emotion, Vector3 position) {
        choiceParticleGenerator.GenerateParticles(position, emotion);
        yield return new WaitForSeconds(ChoiceConstants.choiceSelectionAnimationDuration);

    }
}