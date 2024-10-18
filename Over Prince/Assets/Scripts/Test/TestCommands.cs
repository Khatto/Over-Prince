using UnityEngine;
using UnityEngine.UI;

public class TestCommands : MonoBehaviour
{
    public bool createStartBattleButton = true;
    public bool createForceEnemyToFightButton = true;
    public bool createGameOverButton = true;
    public bool createTestChoiceDialogue = true;


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
    }

    void OnStartBattleButtonClicked()
    {
        // Find the BattleManager in the scene
        BattleManager battleManager = FindObjectOfType<BattleManager>();

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
}