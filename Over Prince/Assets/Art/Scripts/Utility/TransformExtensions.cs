using UnityEngine;

public static class TransformExtensions {
    public static Vector3 FlippedHorizontally(this Vector3 vector) {
        return new Vector3(vector.x * -1, vector.y, vector.z);
    }

    public static Vector3 FlippedVertically(this Vector3 vector) {
        return new Vector3(vector.x, vector.y * -1, vector.z);
    }

}