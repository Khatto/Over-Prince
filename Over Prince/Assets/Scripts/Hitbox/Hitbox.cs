/// <summary>
/// Represents the physical Hitbox of an attack.
/// </summary>
public class Hitbox {
    public float xOffset = 0;
    public float yOffset = 0;
    public float xScale = 1;
    public float yScale = 1;

    public Hitbox(float xOffset, float yOffset, float xScale = 1, float yScale = 1) {
        this.xOffset = xOffset;
        this.yOffset = yOffset;
        this.xScale = xScale;
        this.yScale = yScale;
    }
}

/// <summary>
/// Represents the entity that generates a Hitbox.
/// </summary>
public enum HitboxOwner {
    Player,
    Enemy,
    Environment
}