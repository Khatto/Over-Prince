using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ScaleText : MonoBehaviour
{
    public ShadowedText text;
    public ScaleTextMode scaleTextMode = ScaleTextMode.None;
    public ScaleTextDirection scaleTextDirection = ScaleTextDirection.Up;
    public float scaleTime = 2f;
    public Range2D fontRange = new Vector2(110f, 120f);
    public bool active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = gameObject.GetComponent<ShadowedText>();
        if (active) {
            StartScaling();
        }
    }

    public void StartScaling() {
        active = true;
        switch (scaleTextMode) {
            case ScaleTextMode.ScaleUpAndDown:
                ScaleTextUp();
                break;
        }
    }

    public void RestartScaling() {
        StopScaling();
        text.fontSize = fontRange.min;
        StartScaling();
    }

    public void StopScaling() {
        active = false;
    }
    
    // Create a function that usees a coroutine to scale the font size of a TextMeshPROUGUI object
    public IEnumerator ScaleTextSize(float targetFontSize) {
        float elapsedTime = 0;
        float startFontSize = text.fontSize;
        
        while (elapsedTime < scaleTime) {
            text.fontSize = Mathf.Lerp(startFontSize, targetFontSize, (elapsedTime / scaleTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        text.fontSize = targetFontSize;
        if (scaleTextMode == ScaleTextMode.ScaleUpAndDown) {
            SwitchScaleDirection();
        }
    }

    public void SwitchScaleDirection() {
        if (scaleTextDirection == ScaleTextDirection.Up) {
            ScaleTextDown();
        } else {
            ScaleTextUp();
        }
    }

    public void ScaleTextUp() {
        scaleTextDirection = ScaleTextDirection.Up;
        StartCoroutine(ScaleTextSize(fontRange.max));
    }

    public void ScaleTextDown() {
        scaleTextDirection = ScaleTextDirection.Down;
        StartCoroutine(ScaleTextSize(fontRange.min));
    }
}

public enum ScaleTextMode {
    None,
    ScaleUpAndDown,
}

public enum ScaleTextDirection {
    Up,
    Down,
}