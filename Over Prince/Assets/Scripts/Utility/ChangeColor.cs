using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ChangeColor : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public ShadowedText text;
    public float transitionDuration = 1f;
    public float startTime = 0.0f;

    private Color initialColor;
    public Color targetColor;

    public ChangeColorMode changeColorMode = ChangeColorMode.None;
    public ChangeColorDirection changeColorDirection = ChangeColorDirection.Forward;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (text == null)
        {
            text = GetComponent<ShadowedText>();
        }
    }

    public void SetColorThenChange(Color color)
    {
        initialColor = color;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
        if (text != null)
        {
            text.color = color;
        }
        StartChangingColor();
    }

    public void StartChangingColor()
    {
        startTime = Time.time;
        if (spriteRenderer != null) 
        {
            initialColor = spriteRenderer.color;
        }
        if (text != null) 
        {
            initialColor = text.color;
        }
        changeColorDirection = ChangeColorDirection.Forward;
        StartCoroutine(TransitionColor(initialColor, targetColor));
    }

    private IEnumerator TransitionColor(Color initialColor, Color targetColor)
    {
        if (changeColorMode == ChangeColorMode.None)
        {
            yield break;
        }
        while (Time.time - startTime < transitionDuration && changeColorMode != ChangeColorMode.None)
        {
            if (spriteRenderer != null) 
            {
                spriteRenderer.color = Color.Lerp(initialColor, targetColor, (Time.time - startTime) / transitionDuration);
            }
            if (text != null) 
            {
                text.color = Color.Lerp(initialColor, targetColor, (Time.time - startTime) / transitionDuration);
            }
            yield return null;
        }

        if (spriteRenderer != null) 
        {
            spriteRenderer.color = targetColor;
        }
        if (text != null) 
        {
            text.color = targetColor;
        }

        if (changeColorMode == ChangeColorMode.ChangeColorBackAndForth)
        {
            SwitchColorDirection();
        }
    }

    public void SwitchColorDirection()
    {
        if (changeColorMode == ChangeColorMode.None) {
            return;
        }
        startTime = Time.time;
        if (changeColorDirection == ChangeColorDirection.Forward)
        {
            changeColorDirection = ChangeColorDirection.Backward;
            StartCoroutine(TransitionColor(targetColor, initialColor));
        }
        else if (changeColorDirection == ChangeColorDirection.Backward)
        {
            changeColorDirection = ChangeColorDirection.Forward;
            StartCoroutine(TransitionColor(targetColor, initialColor));
        }
    }

    public void SetTargetColor(Color color)
    {
        targetColor = color;
    }

    public void StopChangingColor()
    {
        StopAllCoroutines();
        changeColorMode = ChangeColorMode.None;
    }
}

public enum ChangeColorMode {
    None,
    ChangeColor,
    ChangeColorBackAndForth
}

public enum ChangeColorDirection {
    Forward,
    Backward
}