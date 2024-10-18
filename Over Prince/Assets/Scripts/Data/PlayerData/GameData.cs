[System.Serializable]
public class GameData {

    PlayerStats playerStats;

    public GameData() {
        playerStats = new PlayerStats(20, 0, 0, 0, 0, 0, 0, 0, 0);
    }
}