using UnityEngine;
using UnityEngine.UI;

public class TestCommands : MonoBehaviour
{
    public bool createStartBattleButton = true;
    public bool createForceEnemyToFightButton = true;


    void OnGUI() {
        if (createStartBattleButton && GUI.Button(new Rect(10, 10, 150, 30), "Start Battle")) { 
            OnStartBattleButtonClicked(); 
        }
        if (createForceEnemyToFightButton && GUI.Button(new Rect(10, 50, 150, 30), "Force Enemy to Fight")) { 
            ForceEnemyToFight(); 
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
}