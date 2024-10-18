using UnityEngine;
using TMPro;

public class ShadowedText : TextMeshProUGUI
{
    public Material shadowMaterial;
    public Color shadowColor;
    public float shadowOffsetX;
    public float shadowOffsetY;
    public ShadowedText shadow;
    public Fade fade;
    public SimpleMovement simpleMovement;

    public class ShadowedTextConstants {
        public const float shadowOffsetX = 0.0f;
        public const float shadowOffsetY = -10.0f;
        public const string tag = "Text Shadow";
    }

    void Start() {
        fade = GetComponent<Fade>();
        simpleMovement = GetComponent<SimpleMovement>();
    }

    public void SetupShadowText() {
        shadow = Instantiate(this, transform.parent);
        shadow.name = name + " Shadow";
        shadow.tag = ShadowedTextConstants.tag;
        shadow.transform.SetSiblingIndex(0);
        shadow.transform.position = transform.position + new Vector3(shadowOffsetX, shadowOffsetY, 0);
        shadow.color = shadowColor;
        shadow.fontMaterial = shadowMaterial;
        shadow.SetText(text);
        shadow.fade = shadow.GetComponent<Fade>();
        shadow.simpleMovement = shadow.GetComponent<SimpleMovement>();
    }

    public void SetText(string text) {
        base.SetText(text);
        if (shadow != null) {
            shadow.SetText(text);
        }
        SetDialoguePositionBasedOnLines(rectTransform.anchoredPosition);
    }

    public void StartFadeWithTime(FadeType fadeType, float time) {
        fade.StartFadeWithTime(fadeType, time);
        if (shadow != null) {
            shadow.StartFadeWithTime(fadeType, time);
        }
    }

    public void SetMovementValues(float movementTime, Vector2 movementDelta, System.Func<float, float> easingFunction) {
        simpleMovement.SetMovementValues(movementTime, movementDelta, easingFunction);
        if (shadow != null) {
            shadow.SetMovementValues(movementTime, movementDelta, easingFunction);
        }
    }

    public void Move() {
        simpleMovement.Move();
        if (shadow != null) {
            shadow.Move();
        }
    }

    public void SetColor(Color color) {
        base.color = color;
        if (shadow != null) {
            shadow.color = color;
        }
    }

    public void SetDialoguePositionBasedOnLines(Vector2 position) {
        rectTransform.anchoredPosition = position;
        if (shadow != null) {
            shadow.rectTransform.anchoredPosition = position + new Vector2(shadowOffsetX, shadowOffsetY);
        }
    }
}