using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScene : MonoBehaviour, IGameEventListener, IBattleEventListener
{
    public static class GameplaySceneConstants {
        public static float sceneFadeInDuration = 1.5f;

        public static class DeathSequence {
            public static float deathSequenceWaitTime = 2.5f;
            public static float screenFaderTime = 0.5f;
            public static float cameraScaleSize = 0.85f;
            public static float screenFaderTargetAlpha = 0.82f;
            public static float gameOverFadeInTime = 0.75f;
            public static float timeScale = 0.85f;
            public static Color screenFaderGameOverStartColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            public static float delayBeforeButtonsAppear = 0.85f;
            public static float buttonFadeInTime = 0.5f;
            public static float scenFadeOutDuration = 0.5f;
        }
    }

    public bool shouldAutoFadeInScene = true;
    public bool shouldAutoDisplayMusicInfo = true;
    public Fade screenFader;
    public GameplaySceneState state = GameplaySceneState.Loading;
    public PlayerController playerController;
    public MusicInfoManager musicInfoManager;
    public ShadowedText gameOverText;
    public SpriteRenderer mapProceedIndicator;
    public Button retryButton;
    public Button returnToTitleButton;
    public Fade screenTransitionFader;

    public virtual void Start()
    {
        if (shouldAutoFadeInScene) {            
            FadeInScene();
            if (shouldAutoDisplayMusicInfo) {
                StartCoroutine(DisplayMusicInfo());
            }
        }
        gameOverText.SetupShadowText();
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

    public virtual void OnGameEvent(GameEvent gameEvent)
    {
        // Implement the game event logic here
    }

    public virtual void OnBattleStart()
    {
        // Implement the battle start logic here
    }

    public virtual void OnBattleComplete()
    {
        // Implement the battle end logic here
    }

    public virtual void OnPlayerDied()
    {
        Time.timeScale = GameplaySceneConstants.DeathSequence.timeScale;
        playerController.animator.SetTrigger(Constants.AnimationKeys.DeathAnimation);
        playerController.DisableOnPlayerDeath();
        CameraZoom zoom = Camera.main.GetComponent<CameraZoom>();
        zoom.SetZoomAndStart(Camera.main.orthographicSize, Camera.main.orthographicSize * GameplaySceneConstants.DeathSequence.cameraScaleSize, GameplaySceneConstants.DeathSequence.deathSequenceWaitTime);
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence() {
        yield return new WaitForSeconds(GameplaySceneConstants.DeathSequence.deathSequenceWaitTime);
        screenFader.targetAlpha = GameplaySceneConstants.DeathSequence.screenFaderTargetAlpha;
        screenFader.useTargetAlpha = true;
        screenFader.SetColor(GameplaySceneConstants.DeathSequence.screenFaderGameOverStartColor);
        screenFader.StartFadeWithTime(FadeType.FadeIn, GameplaySceneConstants.DeathSequence.screenFaderTargetAlpha);
        gameOverText.StartFadeWithTime(FadeType.FadeIn, GameplaySceneConstants.DeathSequence.gameOverFadeInTime);
        yield return new WaitForSeconds(GameplaySceneConstants.DeathSequence.delayBeforeButtonsAppear);
        PerformFadeOnButton(retryButton, FadeType.FadeIn, GameplaySceneConstants.DeathSequence.buttonFadeInTime);
        PerformFadeOnButton(returnToTitleButton, FadeType.FadeIn, GameplaySceneConstants.DeathSequence.buttonFadeInTime);
        retryButton.interactable = true;
        returnToTitleButton.interactable = true;
    }

    private void PerformFadeOnButton(Button button, FadeType fadeType, float fadeTime) {
        Fade childFade = button.GetComponentInChildren<Fade>();
        button.GetComponent<Fade>().StartFadeWithTime(fadeType, fadeTime);
        button.transform.GetChild(0).GetComponent<Fade>().StartFadeWithTime(fadeType, fadeTime); // For some reason button.GetComponentInChildren<Fade>().StartFadeWithTime(fadeType, fadeTime); was behaving strangely.  May need to look into it further.
    }

    public void DisplayMapProceedIndicator(bool shouldDisplay) {
        if (shouldDisplay) {
            mapProceedIndicator.gameObject.SetActive(true);
            mapProceedIndicator.GetComponent<ChangeColor>().SetColorThenChange(Color.white, null, ChangeColorMode.ChangeColorBackAndForth);
        } else {
            mapProceedIndicator.GetComponent<ChangeColor>().StopChangingColor();
            mapProceedIndicator.GetComponent<Fade>().StartFade(FadeType.FadeOut);
        }
    }

    public void LoadNextScene(string sceneName) {
        if (state == GameplaySceneState.Transitioning) return;
        state = GameplaySceneState.Transitioning;
        retryButton.interactable = false;
        returnToTitleButton.interactable = false;
        StartCoroutine(FadeOutSceneThenTransition(sceneName));
    }

    public IEnumerator FadeOutSceneThenTransition(string sceneName) {
        screenTransitionFader.gameObject.SetActive(true);
        screenTransitionFader.StartFadeWithTime(FadeType.FadeIn, GameplaySceneConstants.DeathSequence.scenFadeOutDuration);
        yield return new WaitForSeconds(GameplaySceneConstants.DeathSequence.scenFadeOutDuration);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName == null || sceneName.Length == 0 ? UnityEngine.SceneManagement.SceneManager.GetActiveScene().name : sceneName);
    }
}

public enum GameplaySceneState {
    Loading,
    FadingIn,
    SceneSpecific,
    Playing,
    Paused,
    FadingOut,
    Transitioning
}