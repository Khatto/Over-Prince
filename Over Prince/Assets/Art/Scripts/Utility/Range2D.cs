using UnityEngine;

[System.Serializable]
public struct Range2D
{
    public float min;
    public float max;

    public static implicit operator Range2D(Vector2 value)
    {
        return new Range2D() { min = value.x, max = value.y };
    }
}
