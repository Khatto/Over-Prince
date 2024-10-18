using UnityEngine;

using UnityEngine;

public class CameraRectManager : MonoBehaviour
{
    void Start()
    {
        // Target aspect ratio
        float targetAspect = 16.0f / 9.0f;

        // Current screen aspect ratio
        float windowAspect = (float) Screen.width / (float)Screen.height;

        // Scale height based on the target aspect ratio
        float scaleHeight = windowAspect / targetAspect;

        // Adjust the camera rect to maintain the 16:9 aspect ratio
        if (scaleHeight < 1.0f)
        {
            // Add letterboxing
            Rect rect = Camera.main.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            Camera.main.rect = rect;
        }
        else
        {
            // Add pillarboxing
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = Camera.main.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            Camera.main.rect = rect;
        }
    }
}