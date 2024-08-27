using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public SpriteRenderer spriteRenderer;
    public Image image;

    public bool active = false;
    public FadeType fadeType = FadeType.FadeInFadeOut;
    public float targetAlpha = 0.0f;
    public bool useTargetAlpha = false;

    public float fadeTime = 1f;
    private float startTime = 0.0f;
    private Color startColor;
    private Action callback = null;
    

    void Start () {
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        image = gameObject.GetComponent<Image>();
    }

    void Update () {
        if (active) {
            switch (fadeType) {
                case FadeType.FadeIn:
                    FadeIn();
                    break;
                case FadeType.FadeOut:
                    FadeOut();
                    break;
                case FadeType.FadeInFadeOut:
                    FadeInFadeOut();
                    break;
                case FadeType.FlashInThenFadeOut:
                    FlashInThenFadeOut();
                    break;
            }
        }
    }

    public void StartFadeWithTime (FadeType fadeType, float fadeTime, Action callback = null) {
        this.fadeTime = fadeTime;
        this.callback = callback;
        StartFade(fadeType);
    }

    public void StartFade (FadeType fadeType) {
        this.fadeType = fadeType;
        startTime = Time.time;
        if (textMeshPro != null) {
            startColor = textMeshPro.color;
        }
        if (spriteRenderer != null) {
            startColor = spriteRenderer.color;
        }
        if (image != null) {
            startColor = image.color;
        }
        active = true;
    }

    private void FadeIn () {
        if (textMeshPro != null) {
            Color color = textMeshPro.color;
            color.a = Mathf.Lerp(startColor.a, 1, (Time.time - startTime) / fadeTime);
            textMeshPro.color = color;
            if (color.a == 1.0f) {
                FinishedFadeAction();
            }
        }
        if (spriteRenderer != null) {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(startColor.a, 1, (Time.time - startTime) / fadeTime);
            spriteRenderer.color = color;
            if (color.a == 1.0f) {
                FinishedFadeAction();
            }
        }
        if (image != null) {
            Color color = image.color;
            color.a = Mathf.Lerp(startColor.a, 1, (Time.time - startTime) / fadeTime);
            image.color = color;
            if (color.a == 1.0f) {
                FinishedFadeAction();
            }
        }
    }

    private void FadeOut () {
        if (textMeshPro != null) {
            Color color = textMeshPro.color;
            color.a = Mathf.Lerp(startColor.a, 0, (Time.time - startTime) / fadeTime);
            textMeshPro.color = color;
            if (color.a == 0.0f) {
                FinishedFadeAction();
            }
        }
        if (spriteRenderer != null) {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(startColor.a, 0, (Time.time - startTime) / fadeTime);
            spriteRenderer.color = color;
            if (color.a == 0.0f) {
                FinishedFadeAction();
            }
        }
        if (image != null) {
            Color color = image.color;
            color.a = Mathf.Lerp(startColor.a, 0, (Time.time - startTime) / fadeTime);
            image.color = color;
            if (color.a == 0.0f) {
                FinishedFadeAction();
            }
        }
    }

    private void FadeInFadeOut () {
        if (textMeshPro != null) {
            Color color = textMeshPro.color;
            color.a = Mathf.PingPong(Time.time, fadeTime);
            textMeshPro.color = color;
        }
        if (spriteRenderer != null) {
            Color color = spriteRenderer.color;
            color.a = Mathf.PingPong(Time.time, fadeTime);
            spriteRenderer.color = color;
        }
        if (image != null) {
            Color color = image.color;
            color.a = Mathf.PingPong(Time.time, fadeTime);
            image.color = color;
        }
    }

    private void FlashInThenFadeOut () {
        if (textMeshPro != null) {
            Color color = textMeshPro.color;
            color.a = 1.0f;
            textMeshPro.color = color;
        }
        if (spriteRenderer != null) {
            Color color = spriteRenderer.color;
            color.a = 1.0f;
            spriteRenderer.color = color;
        }
        if (image != null) {
            Color color = image.color;
            color.a = 1.0f;
            image.color = color;
        }
        StartFade(FadeType.FadeOut);
    }

    private void FinishedFadeAction() {
        active = false;
        if (callback != null) {
            callback();
        }
    }

    public void FadeOutAfterDelay(float delay) {
        StartCoroutine(FadeOutAfterDelayCoroutine(delay));
    }

    private IEnumerator FadeOutAfterDelayCoroutine(float delay) {
        yield return new WaitForSeconds(delay);
        StartFade(FadeType.FadeOut);
    }

    public bool HasFadedIn () {
        if (textMeshPro != null) {
            return textMeshPro.color.a == 1.0f;
        }
        if (spriteRenderer != null) {
            return spriteRenderer.color.a == 1.0f;
        }
        if (image != null) {
            return image.color.a == 1.0f;
        }
        return false;
    }

    public bool HasFadedOut () {
        if (textMeshPro != null) {
            return textMeshPro.color.a == 0.0f;
        }
        if (spriteRenderer != null) {
            return spriteRenderer.color.a == 0.0f;
        }
        if (image != null) {
            return image.color.a == 0.0f;
        }
        return false;
    }
}

public enum FadeType {
    FadeIn,
    FadeOut,
    FadeInFadeOut,
    FlashInThenFadeOut
}