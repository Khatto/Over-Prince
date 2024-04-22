using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class IntroSceneManager : MonoBehaviour
{
    public Camera mainCamera;
    public VideoPlayer videoPlayer;
    public SpriteRenderer videoSpriteBackground;
    public Fade screenFader;

    public DialogueManager dialogueManager;
    public IntroSceneState state = IntroSceneState.NotStarted;

    public float startTime = 0.0f;

    public Transform memoryFieldView;
    public ParticleSystem deltaParticles;

    private struct IntroSceneConstants {
        public const float screenFadeDelay = 1.0f;
        public const float cameraDefaultSize = 5.0f;

        public struct PartOne {
            public const float cameraStartY = 1.65f;
            public const float cameraEndY = 1.0f;
            public const float cameraStartSize = 3.5f;
            public const float cameraEndSize = 9.0f;
            public const float cameraZoomTime = 3.0f; // Originally 11 seconds
        }

        public struct PartTwo {
            public const float screenFadeTime = 1.0f;
        }
    }

    void Start()
    {
        deltaParticles.Stop();
        screenFader.FadeOutAfterDelay(IntroSceneConstants.screenFadeDelay);
        PerformPart(IntroSceneState.PartOne);
    }

    void Update() {
        switch (state) {
            case IntroSceneState.PartOne:
                mainCamera.orthographicSize = Mathf.Lerp(
                    IntroSceneConstants.PartOne.cameraStartSize,
                    IntroSceneConstants.PartOne.cameraEndSize, 
                    (Time.time - startTime) / IntroSceneConstants.PartOne.cameraZoomTime
                );
                if (mainCamera.orthographicSize >= IntroSceneConstants.PartOne.cameraEndSize) {
                    PerformPart(IntroSceneState.PartTwo);
                }
                break;
            case IntroSceneState.PartTwo:
                if (screenFader.HasFadedIn()) {
                    PerformPart(IntroSceneState.PartThree);
                }
                break;
            case IntroSceneState.PartFour:
                if (screenFader.HasFadedOut()) {
                    //PerformPart(IntroSceneState.PartThree);
                }
                break;
        }
    }

    private void PerformPart(IntroSceneState state) {
        Debug.Log("Starting state " + state + " at " + Time.time);
        this.state = state;
        switch (state) {
            case IntroSceneState.PartOne:
                startTime = Time.time;
                StartCoroutine(PlayIntroVideo());
                break;
            case IntroSceneState.PartTwo:
                screenFader.StartFadeWithTime(FadeType.FadeIn, IntroSceneConstants.PartTwo.screenFadeTime);
                break;
            case IntroSceneState.PartThree:
                mainCamera.orthographicSize = IntroSceneConstants.cameraDefaultSize;
                videoPlayer.Stop();
                videoPlayer.transform.parent.gameObject.SetActive(false);
                memoryFieldView.gameObject.SetActive(true);
                deltaParticles.Play();
                screenFader.StartFadeWithTime(FadeType.FadeOut, IntroSceneConstants.PartTwo.screenFadeTime);
                break;
            case IntroSceneState.PartFour:
                break;
        }
    }

    IEnumerator PlayIntroVideo() {
        yield return new WaitForSeconds(IntroSceneConstants.screenFadeDelay);        
        videoPlayer.Play();
    }
}

public enum IntroSceneState {
    NotStarted,
    PartOne, // Zooming into protagonist
    PartTwo, // Scrolling up and fading out screen
    PartThree, // Fading out and showing memory: Field
    PartFour, // Fading in and showing memory: Field
    PartFive,
}