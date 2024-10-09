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
                portraitNormalFade.StartFadeWithTime(FadeType.FadeIn, HPPortraitManagerConstants.portraitActionChangeTime);
                portraitHurtFade.StartFadeWithTime(FadeType.FadeOut, HPPortraitManagerConstants.portraitActionChangeTime);
                break;
            case PortraitAction.Hurt:
                portraitNormalFade.StartFadeWithTime(FadeType.FadeOut, HPPortraitManagerConstants.portraitActionChangeTime);
                portraitHurtFade.StartFadeWithTime(FadeType.FadeIn, HPPortraitManagerConstants.portraitActionChangeTime);
                portraitBackground.SetColorThenChange(HPPortraitManagerConstants.portraitHurtColor, HPPortraitManagerConstants.portraitBackgroundColor);
                StartCoroutine(PerformPortraitActionWithDelay(PortraitAction.Normal, HPPortraitManagerConstants.portraitActionReturnDelay));
                break;
            case PortraitAction.Die:
                portraitNormalFade.StartFadeWithTime(FadeType.FadeOut, HPPortraitManagerConstants.portraitActionChangeTime);
                portraitHurtFade.StartFadeWithTime(FadeType.FadeIn, HPPortraitManagerConstants.portraitActionChangeTime);
                portraitBackground.SetColorThenChange(HPPortraitManagerConstants.portraitHurtColor, HPPortraitManagerConstants.portraitBackgroundColor);
                break;
        }
    }

    private IEnumerator PerformPortraitActionWithDelay(PortraitAction action, float delay) {
        yield return new WaitForSeconds(delay);
        PerformPortraitAction(action);
    }

    public void FadePortrait(FadeType fadeType) {
        portraitFrameFade.StartFadeWithTime(fadeType, HPPortraitManagerConstants.portraitFadeTime);
        portraitShadowFade.StartFadeWithTime(fadeType, HPPortraitManagerConstants.portraitFadeTime);
        portraitNormalFade.StartFadeWithTime(fadeType, HPPortraitManagerConstants.portraitFadeTime);
        if (fadeType == FadeType.FadeOut) {
            portraitHurtFade.StartFadeWithTime(fadeType, HPPortraitManagerConstants.portraitFadeTime);
        }
        portraitBackgroundFade.useTargetAlpha = fadeType == FadeType.FadeIn;
        portraitBackgroundFade.StartFadeWithTime(fadeType, HPPortraitManagerConstants.portraitFadeTime);
    }
}

public enum PortraitAction {
    Normal,
    Hurt,
    Die
}