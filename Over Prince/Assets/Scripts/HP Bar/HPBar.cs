using UnityEngine;

public class HPBar : MonoBehaviour
{
    public bool displayHPBar = true;
    public bool shouldShiftBar = true;
    public Transform currentHPBar;
    public SpriteRenderer currentHPBarSpriteRenderer;

    public Transform backgroundBar;

    public Transform lossIndicatorTransform;
    public SpriteRenderer lossIndicator;
    public Fade lossIndicatorFade;

    public Fade currentHPBarFade;
    public Fade backgroundBarFade;
    public Fade backgroundBorderFade;
    public Fade barShadowFade;

    public Vector2 barOffset = new Vector2(0, 0);

    public int maxHP = 100;
    public int currentHP = 100;

    private int lastDamageAmount = 0;

    public static class Constants
    {
        public const float hpBarFadeTime = 0.5f;
        public const float lossIndicatorFadeTime = 0.25f;
        public static Color currentHPMaxColor = new Color(0.1764706f, 0.7450981f, 0.03137255f, 0.8f);
        public static Color backgroundBarColor = new Color(0.3113208f, 0.3113208f, 0.3113208f, 0.6392157f);
    }

    void Start()
    {
        currentHPBarFade = currentHPBar.GetComponent<Fade>();
        backgroundBarFade = backgroundBar.GetComponent<Fade>();
        currentHPBarSpriteRenderer = currentHPBar.GetComponent<SpriteRenderer>();
        lossIndicator = lossIndicatorTransform.GetComponent<SpriteRenderer>();
        lossIndicatorFade = lossIndicatorTransform.GetComponent<Fade>();
        UpdateHPBar();
        if (shouldShiftBar) SetBarPositions();
        if (!displayHPBar)
        {
            currentHPBar.GetComponent<SpriteRenderer>().enabled = false;
            backgroundBar.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void Update() {
        if (displayHPBar && transform.parent != null) FlipHPBarDependingOnParentFacing();
    }

    public void DisplayHPBar() {
        displayHPBar = true;
        currentHPBar.GetComponent<SpriteRenderer>().enabled = true;
        backgroundBar.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void FadeInHPBar() {
        currentHPBarFade.StartFadeWithTime(FadeType.FadeIn, Constants.hpBarFadeTime);
        backgroundBarFade.StartFadeWithTime(FadeType.FadeIn, Constants.hpBarFadeTime);
        backgroundBorderFade.StartFadeWithTime(FadeType.FadeIn, Constants.hpBarFadeTime);
        barShadowFade.StartFadeWithTime(FadeType.FadeIn, Constants.hpBarFadeTime);
    }

    public void FlipHPBarDependingOnParentFacing() {
        float localParentScaleX = transform.parent.localScale.x;
        float facingMultiplier = localParentScaleX < 0 ? -1 : 1;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * facingMultiplier, 1, 1);
    }

    public void Setup(int maxHP)
    {
        this.maxHP = maxHP;
        currentHP = maxHP;
        UpdateHPBar();
    }

    void SetBarPositions()
    {
        currentHPBar.position = new Vector3(currentHPBar.position.x - currentHPBar.GetComponent<SpriteRenderer>().size.x + barOffset.x, currentHPBar.position.y + barOffset.y, currentHPBar.position.z);
        backgroundBar.position = currentHPBar.position;
    }


    public void ChangeHP(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        else if (currentHP < 0)
        {
            currentHP = 0;
        }
        lastDamageAmount = amount;
        if (displayHPBar) 
        {
            UpdateHPBar();
            UpdateLossIndicator();
        }
    }

    private void UpdateHPBar()
    {
        if (displayHPBar) {
            float scale = (float) currentHP / (float) maxHP;
            currentHPBar.localScale = new Vector3(scale, 1f, 1f);
        }
    }

    private void UpdateLossIndicator()
    {
        if (lastDamageAmount < 0)
        {
            lossIndicatorTransform.localScale = new Vector3((float) lastDamageAmount / (float) maxHP, 1f, 1f);
            float rightSideOfHPBar = currentHPBar.localPosition.x + currentHPBar.localScale.x * currentHPBarSpriteRenderer.size.x;
            float lossIndicatorWidth = lossIndicator.size.x * Mathf.Abs(lossIndicatorTransform.localScale.x);
            lossIndicatorTransform.localPosition = new Vector3(rightSideOfHPBar + lossIndicatorWidth, lossIndicatorTransform.localPosition.y, lossIndicatorTransform.localPosition.z);
            lossIndicatorFade.StartFadeWithTime(FadeType.FlashInThenFadeOut, Constants.lossIndicatorFadeTime);
        }
    }

    public void FadeOut()
    {
        currentHPBarFade.StartFadeWithTime(FadeType.FadeOut, Constants.hpBarFadeTime);
        backgroundBarFade.StartFadeWithTime(FadeType.FadeOut, Constants.hpBarFadeTime);
    }
}
