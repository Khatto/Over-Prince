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

    public static float Cubic(float t)
    {
        return t * t * t;
    }

    public static float InverseCubic(float t)
    {
        return 1 - Cubic(1 - t);
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

public enum EasingFunctionType {
    Linear,
    EaseIn,
    EaseOut,
    Cubic,
    InverseCubic,
    EaseInOut
}

static class EasingFunctionTypeMethods {
    public static System.Func<float, float> GetEasingFunction(this EasingFunctionType easingFunction) {
        switch (easingFunction) {
            case EasingFunctionType.Linear:
                return EasingFuncs.Linear;
            case EasingFunctionType.EaseIn:
                return EasingFuncs.EaseIn;
            case EasingFunctionType.EaseOut:
                return EasingFuncs.EaseOut;
            case EasingFunctionType.Cubic:
                return EasingFuncs.Cubic;
            case EasingFunctionType.InverseCubic:
                return EasingFuncs.InverseCubic;
            case EasingFunctionType.EaseInOut:
                return EasingFuncs.EaseInOut;
            default:
                return EasingFuncs.Linear;
        }
    }
}