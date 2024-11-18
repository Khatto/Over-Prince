using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCommands : MonoBehaviour
{
    public bool createStartBattleButton = true;
    public bool createForceEnemyToFightButton = true;
    public bool createGameOverButton = true;
    public bool createTestChoiceDialogue = true;

    public bool createApplyHitButton = true;
    public int hitDamage = 1;

    public bool createStartSpeakingButton = true;
    public List<OrganicMouth> organicMouths = new List<OrganicMouth>();

    public bool createTestDoubleDialogueButton = true;
    public bool createTestTripleDialogueButton = true;
    public bool createTransportToHoodedBoyEventButton = true;


    void OnGUI() {
        if (createStartBattleButton && GUI.Button(new Rect(10, 10, 150, 30), "Start Battle")) { 
            OnStartBattleButtonClicked(); 
        }
        if (createForceEnemyToFightButton && GUI.Button(new Rect(10, 50, 150, 30), "Force Enemy to Fight")) { 
            ForceEnemyToFight(); 
        }
        if (createGameOverButton && GUI.Button(new Rect(10, 90, 150, 30), "Game Over")) { 
            OnGameOverButtonClicked(); 
        }
        if (createGameOverButton && GUI.Button(new Rect(10, 130, 150, 30), "Start Choice Dialogue")) { 
            OnCreateTestChoiceDialogue(); 
        }
        if (createApplyHitButton && GUI.Button(new Rect(10, 170, 150, 30), "Apply Hit")) { 
            OnApplyHitButtonClicked(); 
        }
        if (createStartSpeakingButton && GUI.Button(new Rect(10, 210, 150, 30), "Start Speaking")) { 
            OnStartSpeakingButtonClicked(); 
        }
        if (createTestDoubleDialogueButton && GUI.Button(new Rect(10, 250, 150, 30), "Test Double Dialogue")) { 
            OnCreateTestDialogue(2); 
        }
        if (createTestTripleDialogueButton && GUI.Button(new Rect(10, 290, 150, 30), "Test Triple Dialogue")) { 
            OnCreateTestDialogue(3); 
        }
        if (createTransportToHoodedBoyEventButton && GUI.Button(new Rect(10, 330, 150, 30), "Go to Hooded Boy")) { 
            OnTransportToHoodedBoyEvent();
        }
    }

    void OnStartBattleButtonClicked()
    {
        // Find the BattleManager in the scene
        BattleManager battleManager = FindFirstObjectByType<BattleManager>();

        if (battleManager != null)
        {
            // Start the battle
            battleManager.StartBattle();
        }
        else
        {
            Debug.LogError("BattleManager not found in the scene.");
        }
    }

    void ForceEnemyToFight()
    {
        // Find the TriangleSlime in the scene
        Enemy triangleSlime = FindFirstObjectByType<Enemy>();

        if (triangleSlime != null)
        {
            // Force the TriangleSlime to enter the Idle state
            triangleSlime.EnterState(CharacterState.Idle);
        }
        else
        {
            Debug.LogError("TriangleSlime not found in the scene.");
        }
    }

    void OnGameOverButtonClicked()
    {
        // Find the BattleManager in the scene
        BattleManager battleManager = FindFirstObjectByType<BattleManager>();

        if (battleManager != null)
        {
            // End the battle
            battleManager.PlayerDied();
        }
        else
        {
            Debug.LogError("BattleManager not found in the scene.");
        }
    }

    void OnCreateTestChoiceDialogue()
    {
        DialogueManager dialogueManager = FindFirstObjectByType<DialogueManager>();
        dialogueManager.DisplayDialogues(DialogueConstants.FieldLobbyIntro.HoodedBoyEncounter.dialogues);
    }

    void OnApplyHitButtonClicked()
    {
        // Find the Player in the scene
        Player player = FindPlayer();

        if (player != null)
        {
            var hit = new Hit(1, 1, 1, 1, 1, hitDamage, 1, 1, new Vector2(0, 0));
            // Apply a hit to the Player
            player.ApplyHit(hit, Constants.Direction.Left);
        }
    }
    
    void OnStartSpeakingButtonClicked()
    {
        for (int i = 0; i < organicMouths.Count; i++)
        {
            organicMouths[i].Speak(true);
        }
    }

    void OnCreateTestDialogue(int amount) { 
        DialogueManager dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (amount == 2) {
            dialogueManager.DisplayDialogues(DialogueConstants.TestDialogue.doubleChoiceTest);
        }
        else if (amount == 3) {
            dialogueManager.DisplayDialogues(DialogueConstants.TestDialogue.tripleChoiceTest);
        }
    }

    public Player FindPlayer()
    {
        return FindFirstObjectByType<Player>();
    }

    void OnTransportToHoodedBoyEvent()
    {
        Player player = FindPlayer();
        player.transform.position = new Vector3(36, -2, 0);
        FileLobbyIntroStageManager manager = FindFirstObjectByType<FileLobbyIntroStageManager>();
        manager.PerformSceneAction(FileLobbyIntroStageState.NavigateTowardsEnd);
    }
}