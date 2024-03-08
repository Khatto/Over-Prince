/// <summary>
/// Represents the physical Hitbox of an attack.
/// </summary>
public class Hitbox {
    public float xOffset = 0;
    public float yOffset = 0;

    public Hitbox(float xOffset, float yOffset) {
        this.xOffset = xOffset;
        this.yOffset = yOffset;
    }
}