using UnityEngine;
public static class EasingFuncs {
    public static float Linear(float t)
    {
        return t;
    }

    public static float EaseIn(float t)
    {
        return t * t;
    }

    public static float EaseOut(float t)
    {
        return Flip(EaseIn(Flip(t)));
    }

    public static float EaseInOut(float t)
    {
        return Mathf.Lerp(EaseIn(t), EaseOut(t), t);
    }

    private static float Flip(float x)
    {
        return 1 - x;
    }
}