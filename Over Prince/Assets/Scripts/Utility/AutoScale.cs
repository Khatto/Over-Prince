using UnityEngine;
using UnityEngine.UIElements;

public class AutoScale : MonoBehaviour
{
    public AutoScaleHorizontalMode autoScaleHorizontalMode = AutoScaleHorizontalMode.None;
    public AutoScalVerticalMode autoScaleVerticalMode = AutoScalVerticalMode.None;
    public bool useBaseHeight = false;
    public float baseHeight;
    public ScaleUpdateFrequency scaleUpdateFrequency = ScaleUpdateFrequency.OnStart;

    public float verticalHeightPercentage = 1;
    public float horizontalHeightPercentage = 1;
    public float horizontalScalePadding = 0;
    public float verticalScalePadding = 0;

    public void Start() {
        if (scaleUpdateFrequency == ScaleUpdateFrequency.OnStart)
        {
            Scale();
        }
    }

    public void Update() {
        if (scaleUpdateFrequency == ScaleUpdateFrequency.OnUpdate)
        {
            Scale();
        }
    }

    public void Scale() {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float worldScreenHeight = (useBaseHeight ? baseHeight : Camera.main.orthographicSize) * 2.0f;

            switch (autoScaleHorizontalMode)
            {
                case AutoScaleHorizontalMode.MatchViewportWidth:
                    float worldScreenWidth = worldScreenHeight * Camera.main.aspect;
                    transform.localScale = new Vector3(worldScreenWidth / spriteRenderer.size.x + horizontalScalePadding, transform.localScale.y, transform.localScale.z);
                    break;
                case AutoScaleHorizontalMode.MatchViewportWidthPercentage:
                    float worldScreenWidthPercentage = worldScreenHeight * Camera.main.aspect * horizontalHeightPercentage;
                    transform.localScale = new Vector3(worldScreenWidthPercentage / spriteRenderer.size.x + horizontalScalePadding, transform.localScale.y, transform.localScale.z);
                    break;
            }

            switch (autoScaleVerticalMode)
            {
                case AutoScalVerticalMode.MatchViewportHeight:
                    transform.localScale = new Vector3(transform.localScale.x, worldScreenHeight / spriteRenderer.size.y + verticalScalePadding, transform.localScale.z);
                    break;
                case AutoScalVerticalMode.MatchViewportHeightPercentage:
                    float worldScreenHeightPercentage = worldScreenHeight * verticalHeightPercentage;
                    transform.localScale = new Vector3(transform.localScale.x, worldScreenHeightPercentage / spriteRenderer.size.y + verticalScalePadding, transform.localScale.z);
                    break;
            }
        }
    }
}

public enum AutoScaleHorizontalMode {
    None,
    MatchViewportWidth,
    MatchViewportWidthPercentage
}

public enum AutoScalVerticalMode {
    None,
    MatchViewportHeight,
    MatchViewportHeightPercentage
}

public enum ScaleUpdateFrequency {
    None,
    OnStart,
    OnUpdate
}