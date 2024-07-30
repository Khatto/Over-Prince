using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScene : MonoBehaviour
{
    public static class GameplaySceneConstants {
        public static float sceneFadeInDuration = 1.5f;
    }

    public bool shouldAutoFadeInScene = true;
    public bool shouldAutoDisplayMusicInfo = true;
    public Fade screenFader;
    public GameplaySceneState state = GameplaySceneState.Loading;
    public PlayerController playerController;
    public MusicInfoManager musicInfoManager;

    public virtual void Start()
    {
        if (shouldAutoFadeInScene) {            
            FadeInScene();
            if (shouldAutoDisplayMusicInfo) {
                StartCoroutine(DisplayMusicInfo());
            }
        }
    }

    /// <summary>
    /// Fades in the scene using the screenFader
    /// </summary>
    void FadeInScene() {
        if (screenFader != null) {
            // For the scenario where the screenFader isn't active but is referenced in the inspector
            screenFader.gameObject.SetActive(true);
        } else {
            // For the scenario where the screenFader isn't manually set in the inspector
            screenFader = GameObject.Find("Screen Fader").GetComponent<Fade>();
        }
        StartSceneFadeIn();
    }

    /// <summary>
    /// Starts the scene fade in
    /// </summary>
    void StartSceneFadeIn() {
        state = GameplaySceneState.FadingIn;
        screenFader.StartFadeWithTime(FadeType.FadeOut, GameplaySceneConstants.sceneFadeInDuration);
    }

    /// <summary>
    /// Displays the music info based on the specific scene
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayMusicInfo() {
        yield return new WaitForSeconds(GameplaySceneConstants.sceneFadeInDuration);
        musicInfoManager.DisplayMusicInfo();
    }

    public virtual void Update()
    {
        if (state == GameplaySceneState.FadingIn && screenFader.HasFadedOut()) {
            StartSceneEntry();
        }
    }

    /// <summary>
    /// This method is meant to be overridden by the child class to handle the scene entry
    /// </summary>
    public virtual void StartSceneEntry() {
        state = GameplaySceneState.Playing;
    }
}

public enum GameplaySceneState {
    Loading,
    FadingIn,
    SceneSpecific,
    Playing,
    Paused,
    FadingOut
}