using UnityEngine;

public class HPBar : MonoBehaviour
{
    public bool displayHPBar = true;
    public bool shouldShiftBar = true;
    public Transform currentHPBar;
    public Transform backgroundBar;

    public Fade currentHPBarFade;
    public Fade backgroundBarFade;

    public Vector2 barOffset = new Vector2(0, 0);

    public int maxHP = 100;
    public int currentHP = 100;

    public static class Constants
    {
        public const float hpBarFadeTime = 0.5f;
    }

    void Start()
    {
        currentHPBarFade = currentHPBar.GetComponent<Fade>();
        backgroundBarFade = backgroundBar.GetComponent<Fade>();
        UpdateHPBar();
        if (shouldShiftBar)
        {
            SetBarPositions();
        }
        if (!displayHPBar)
        {
            currentHPBar.GetComponent<SpriteRenderer>().enabled = false;
            backgroundBar.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void Update() {
        if (displayHPBar) {
            FlipHPBarDependingOnParentFacing();
        }
    }

    public void DisplayHPBar() {
        displayHPBar = true;
        currentHPBar.GetComponent<SpriteRenderer>().enabled = true;
        backgroundBar.GetComponent<SpriteRenderer>().enabled = true;
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
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        if (displayHPBar) {
            
            float scale = (float) currentHP / (float) maxHP;
            currentHPBar.localScale = new Vector3(scale, 1f, 1f);
        }
    }

    public void FadeOut()
    {
        currentHPBarFade.StartFadeWithTime(FadeType.FadeOut, Constants.hpBarFadeTime);
        backgroundBarFade.StartFadeWithTime(FadeType.FadeOut, Constants.hpBarFadeTime);
    }
}
