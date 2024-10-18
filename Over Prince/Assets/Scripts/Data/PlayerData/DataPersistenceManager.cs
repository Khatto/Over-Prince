using System.IO;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private static DataPersistenceManager instance;

    public static class DataPersistenceManagerConstants {
        public const string gameDataFileName = "overPrinceData.json";
    }

    public void SetupSingleton() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private string filePath;

    private void Awake()
    {
        SetupSingleton();
        filePath = Application.persistentDataPath + DataPersistenceManagerConstants.gameDataFileName;
    }

    public void SaveGameData(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, json);
    }

    public GameData LoadGameData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            Debug.LogWarning("No game data file found.");
            return null;
        }
    }
}