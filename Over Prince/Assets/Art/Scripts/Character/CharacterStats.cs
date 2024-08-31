/// <summary>
/// Represents the various statistics of the character, including HP, attack, MP, etc.
/// </summary>
public class CharacterStats {
    public int maxHP;
    public int currentHP;
    public int experience;
    public int experienceToNextLevel;

    public CharacterStats(int maxHP, int experience, int experienceToNextLevel) {
        this.maxHP = maxHP;
        this.currentHP = maxHP;
        this.experience = experience;
        this.experienceToNextLevel = experienceToNextLevel;
    }
}