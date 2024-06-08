using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScene : MonoBehaviour
{
    public static class GameplaySceneConstants {
        public static float sceneFadeInDuration = 1.5f;
    }

    public bool shouldAutoFadeInScene = true;
    public Fade screenFader;
    public GameplaySceneState state = GameplaySceneState.Loading;

    // Start is called before the first frame update
    public virtual void Start()
    {
        if (shouldAutoFadeInScene) {            
            if (screenFader != null) { 
                // For the scenario where the screenFader isn't active but is referenced in the inspector
                screenFader.gameObject.SetActive(true);
            } else { 
                // For the scenario where the screenFader isn't manually set in the inspector
                screenFader = GameObject.Find("Screen Fader").GetComponent<Fade>();
            }

            if (screenFader != null) {
                screenFader.StartFadeWithTime(FadeType.FadeOut, GameplaySceneConstants.sceneFadeInDuration);
            }
        }
    }


    void StartSceneFadeIn() {
        screenFader.StartFadeWithTime(FadeType.FadeIn, GameplaySceneConstants.sceneFadeInDuration);
    }
}

public enum GameplaySceneState {
    Loading,
    FadingIn,
    Playing,
    Paused,
    FadingOut
}