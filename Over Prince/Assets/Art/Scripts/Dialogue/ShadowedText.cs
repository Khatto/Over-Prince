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
    public Fade shadowFade;
    public SimpleMovement shadowSimpleMovement;

    public class ShadowedTextConstants {
        public const float shadowOffsetX = 0.0f;
        public const float shadowOffsetY = -10.0f;
    }

    void Start() {
        fade = GetComponent<Fade>();
        simpleMovement = GetComponent<SimpleMovement>();
    }

    public void SetupShadowText() {
        shadow = Instantiate(this, transform.parent);
        shadow.name = name + " Shadow";
        shadow.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
        shadow.transform.position = transform.position + new Vector3(shadowOffsetX, shadowOffsetY, 0);
        shadow.color = shadowColor;
        shadow.fontMaterial = shadowMaterial;
        shadow.SetText(text);
    }

    public void SetText(string text) {
        base.SetText(text);
        if (shadow != null) {
            shadow.SetText(text);
        }
    }
}