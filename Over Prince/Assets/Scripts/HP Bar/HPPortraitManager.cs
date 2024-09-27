using System.Collections;
using UnityEngine;

public class HPPortraitManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public Fade portraitNormalFade;
    public Fade portraitHurtFade;

    public ChangeColor portraitBackground;
    private Fade portraitBackgroundFade;

    public Fade portraitFrameFade;
    public Fade portraitShadowFade;

    public static class HPPortraitManagerConstants {
        public const float portraitFadeTime = 0.5f;
        public const float portraitActionChangeTime = 0.05f;
        public const float portraitActionReturnDelay = 0.5f;
        public static Color portraitHurtColor = new Color(0.75f, 0.15f, 0, 1);
        public static Color portraitBackgroundColor = new Color(0, 0, 0, 0.75f);
    }
    
    void Start()
    {
        portraitBackgroundFade = portraitBackground.GetComponent<Fade>();
    }

    public void PerformPortraitAction(PortraitAction action) {
        switch (action) {
            case PortraitAction.Normal:
                Debug.Log("Should be performing Portrait Normal action!");
                portraitNormalFade.StartFadeWithTime(FadeType.FadeIn, HPPortraitManagerConstants.portraitActionChangeTime);
                portraitHurtFade.StartFadeWithTime(FadeType.FadeOut, HPPortraitManagerConstants.portraitActionChangeTime);
                break;
            case PortraitAction.Hurt:
                portraitNormalFade.StartFadeWithTime(FadeType.FadeOut, HPPortraitManagerConstants.portraitActionChangeTime);
                portraitHurtFade.StartFadeWithTime(FadeType.FadeIn, HPPortraitManagerConstants.portraitActionChangeTime);
                portraitBackground.SetColorThenChange(HPPortraitManagerConstants.portraitHurtColor, HPPortraitManagerConstants.portraitBackgroundColor);
                StartCoroutine(PerformPortraitActionWithDelay(PortraitAction.Normal, HPPortraitManagerConstants.portraitActionReturnDelay));
                break;
        }
    }

    private IEnumerator PerformPortraitActionWithDelay(PortraitAction action, float delay) {
        yield return new WaitForSeconds(delay);
        PerformPortraitAction(action);
    }

    public void DisplayPortrait() {
        portraitFrameFade.StartFadeWithTime(FadeType.FadeIn, HPPortraitManagerConstants.portraitFadeTime);
        portraitShadowFade.StartFadeWithTime(FadeType.FadeIn, HPPortraitManagerConstants.portraitFadeTime);
        portraitNormalFade.StartFadeWithTime(FadeType.FadeIn, HPPortraitManagerConstants.portraitFadeTime);
        portraitBackgroundFade.StartFadeWithTime(FadeType.FadeIn, HPPortraitManagerConstants.portraitFadeTime);
    }

    public void HidePortrait() {
        portraitFrameFade.StartFadeWithTime(FadeType.FadeOut, HPPortraitManagerConstants.portraitFadeTime);
        portraitShadowFade.StartFadeWithTime(FadeType.FadeOut, HPPortraitManagerConstants.portraitFadeTime);
        portraitNormalFade.StartFadeWithTime(FadeType.FadeOut, HPPortraitManagerConstants.portraitFadeTime);
        portraitBackgroundFade.StartFadeWithTime(FadeType.FadeOut, HPPortraitManagerConstants.portraitFadeTime);
    }
}

public enum PortraitAction {
    Normal,
    Hurt
}