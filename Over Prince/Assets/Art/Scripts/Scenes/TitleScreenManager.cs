using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;

public class TitleScreenManager : MonoBehaviour
{
    public TextMeshProUGUI startLabel;
    public Fade copyrightInfoFade;
    public VideoPlayer videoPlayer;
    public SpriteRenderer videoSpriteBackground;
    public SpriteRenderer titleImage;
    public Fade screenFader; // TODO - Get rid of this if we don't need it
    public ParticleSystem deltaParticles;
    public bool canProceed = false;
    public bool transitioning = false;

    public InputActionAsset actions;
    private InputAction proceedAction;

    internal static class TitleScreenConstants {
        internal static class InputKeyNames {
            internal const string PlayerInput = "playerInput";
            internal const string Proceed = "proceed";
        }
    }

    void Start()
    {
        SetupActions();
        deltaParticles.Stop();
        StartCoroutine(PlayVideo());
        StartCoroutine(DelayTextDisplay());
    }

    void Update() {
        if (!canProceed) {
            CheckForCanProceed();
        }
    }

    private IEnumerator PlayVideo() {
        videoPlayer.loopPointReached += EndReached;
        yield return new WaitForSeconds(1.2f);
        videoPlayer.Play();
    }

    private void EndReached(VideoPlayer videoPlayer)
    {
        videoPlayer.gameObject.SetActive(false);
        titleImage.gameObject.SetActive(true);
        videoSpriteBackground.gameObject.SetActive(false);
        deltaParticles.Play();
    }

    private IEnumerator DelayTextDisplay() {
        yield return new WaitForSeconds(3.8f);
        startLabel.GetComponent<Fade>().StartFade(FadeType.FadeInFadeOut);
        copyrightInfoFade.StartFade(FadeType.FadeIn);
    }

    private void SetupActions() {
        proceedAction = actions.FindActionMap(TitleScreenConstants.InputKeyNames.PlayerInput).FindAction(TitleScreenConstants.InputKeyNames.Proceed);
        proceedAction.performed += (context) => OnProceedPressed(context);
    }

    private void CheckForCanProceed() {
        if (!canProceed && startLabel.color.a >= 0.99f) {
            canProceed = true;
        }
    }

    private void OnProceedPressed(InputAction.CallbackContext context) {
        if (canProceed && !transitioning) {
            TransitionToNextScene();
        }
    }
    private void TransitionToNextScene() {
        transitioning = true;
        screenFader.StartFadeWithTime(FadeType.FadeIn, Constants.Scenes.SceneTransitionTime);
        StartCoroutine(TransitionToNextSceneWithDelay());
    }

    private IEnumerator TransitionToNextSceneWithDelay() {
        yield return new WaitForSeconds(Constants.Scenes.SceneTransitionTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.Scenes.Intro);
    }

    private void OnEnable()
    {
        actions.FindActionMap(TitleScreenConstants.InputKeyNames.PlayerInput).Enable();
    }

    private void OnDisable()
    {
        actions.FindActionMap(TitleScreenConstants.InputKeyNames.PlayerInput).Disable();
    }
}
