[System.Serializable]
public class PlayerStats : CharacterStats {
    public int totalEnemiesDefeated;
    public int totalDamageDealt;
    public int totalDamageTaken;
    public int totalFrenzyPoints;
    public int totalConfusionPoints;
    public int totalSorrowPoints;

    public PlayerStats(int maxHP, int experience, int experienceToNextLevel) : base(maxHP, experience, experienceToNextLevel) {
        totalEnemiesDefeated = 0;
        totalDamageDealt = 0;
        totalDamageTaken = 0;
        totalFrenzyPoints = 0;
        totalConfusionPoints = 0;
        totalSorrowPoints = 0;
    }

    public PlayerStats(int maxHP, int experience, int experienceToNextLevel, int totalEnemiesDefeated, int totalDamageDealt, int totalDamageTaken, int totalFrenzyPoints, int totalConfusionPoints, int totalSorrowPoints) : base(maxHP, experience, experienceToNextLevel) {
        this.totalEnemiesDefeated = totalEnemiesDefeated;
        this.totalDamageDealt = totalDamageDealt;
        this.totalDamageTaken = totalDamageTaken;
        this.totalFrenzyPoints = totalFrenzyPoints;
        this.totalConfusionPoints = totalConfusionPoints;
        this.totalSorrowPoints = totalSorrowPoints;
    }
}