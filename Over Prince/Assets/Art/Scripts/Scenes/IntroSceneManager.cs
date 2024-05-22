using System;
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
    public Transform memoryFileView;
    public Transform sceneSideView;
    public Transform memoryElevatorView;
    public Transform memoryExperiencesView;
    public ParticleSystem deltaParticles;
    public ParticleSystem deltaParticles2;
    public ParticleSystem sideParticlesBack;
    public ParticleSystem sideParticlesFront;
    public SpriteRenderer introImageSad;

    private struct IntroSceneConstants {
        public const float screenFadeDelay = 1.0f;

        public const float cameraZPos = -10.0f;
        public const float cameraDefaultSize = 5.0f;
        public const float screenFadeTime = 1.0f;

        public struct PartOne {
            public const float cameraStartY = 1.65f;
            public const float cameraEndY = 1.0f;
            public const float cameraStartSize = 3.5f;
            public const float cameraEndSize = 9.0f;
            public const float cameraZoomTime = 12.0f;
        }

        public struct PartTwo {
            public const float screenFadeTime = 1.0f;
        }

         public struct PartEight {
            public static Vector2 cameraStartPos = new Vector3(0.0f, 0.0f, cameraZPos);
            public static Vector2 cameraEndPos = new Vector4(4.8f, 0.63f, cameraZPos);
            public const float cameraStartSize = 3.5f;
            public const float cameraEndSize = 4.25f;
            public const float cameraZoomTime = 10.0f;
        }
    }

    void Start()
    {
        PerformPart(IntroSceneState.PartEight);
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
            case IntroSceneState.PartThree:
                if (screenFader.HasFadedOut()) {
                    PerformPart(IntroSceneState.PartFour);
                }
                break;
            case IntroSceneState.PartFour:
                if (dialogueManager.state == DialogueState.Finished) {
                    PerformPart(IntroSceneState.PartFive);
                }
                break;
            case IntroSceneState.PartFive:
                if (screenFader.HasFadedIn()) {
                    PerformPart(IntroSceneState.PartSix);
                }
                break;
            case IntroSceneState.PartSix:
                if (dialogueManager.state == DialogueState.Finished) {
                    PerformPart(IntroSceneState.PartSeven);
                }
                break;
            case IntroSceneState.PartSeven:
                if (screenFader.HasFadedIn()) {
                    PerformPart(IntroSceneState.PartEight);
                }
                break;
            case IntroSceneState.PartEight:
                /*
                mainCamera.orthographicSize = Mathf.Lerp(
                    IntroSceneConstants.cameraDefaultSize,
                    IntroSceneConstants.PartEight.cameraEndSize, 
                    (Time.time - startTime) / IntroSceneConstants.PartEight.cameraZoomTime
                );
                Vector3 cameraPos = Vector3.Lerp(
                    IntroSceneConstants.PartEight.cameraStartPos,
                    IntroSceneConstants.PartEight.cameraEndPos,
                    (Time.time - startTime) / IntroSceneConstants.PartEight.cameraZoomTime
                );
                cameraPos.z = IntroSceneConstants.cameraZPos;
                mainCamera.transform.position = cameraPos;
                */
                if (mainCamera.orthographicSize <= IntroSceneConstants.PartEight.cameraEndSize) {
                    PerformPart(IntroSceneState.PartNine);
                }
                break;
            case IntroSceneState.PartNine:
                if (screenFader.HasFadedIn()) {
                    PerformPart(IntroSceneState.PartTen);
                }
                break;
            case IntroSceneState.PartTen:
                if (dialogueManager.state == DialogueState.Finished) {
                    PerformPart(IntroSceneState.PartEleven);
                }
                break;
            case IntroSceneState.PartEleven:
                if (screenFader.HasFadedIn()) {
                    PerformPart(IntroSceneState.PartTwelve);
                }
                break;
            case IntroSceneState.PartTwelve:
                if (dialogueManager.state == DialogueState.Finished) {
                    PerformPart(IntroSceneState.PartThirteen);
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
                deltaParticles.Stop();
                screenFader.FadeOutAfterDelay(IntroSceneConstants.screenFadeDelay);
                videoPlayer.transform.parent.gameObject.SetActive(true);
                StartCoroutine(PlayIntroVideo());
                StartCoroutine(
                    DelayThenDisplayDialogues(
                        DialogueConstants.IntroScene.PartOne.dialogues,
                        IntroSceneConstants.screenFadeDelay * 2.0f
                    )
                );
                break;
            case IntroSceneState.PartTwo:
                screenFader.StartFadeWithTime(FadeType.FadeIn, IntroSceneConstants.screenFadeTime);
                break;
            case IntroSceneState.PartThree:
                mainCamera.orthographicSize = IntroSceneConstants.cameraDefaultSize;
                videoPlayer.Stop();
                videoPlayer.transform.parent.gameObject.SetActive(false);
                memoryFieldView.gameObject.SetActive(true);
                deltaParticles.Play();
                screenFader.StartFadeWithTime(FadeType.FadeOut, IntroSceneConstants.screenFadeTime);
                break;
            case IntroSceneState.PartFour:
                MoveParallaxContainer(memoryFieldView, DialogueConstants.IntroScene.PartFour.dialogues, IntroSceneConstants.screenFadeTime);
                dialogueManager.DisplayDialogues(DialogueConstants.IntroScene.PartFour.dialogues);
                break;
            case IntroSceneState.PartFive:
                screenFader.StartFadeWithTime(FadeType.FadeIn, IntroSceneConstants.screenFadeTime);
                break;
            case IntroSceneState.PartSix:
                deltaParticles.Stop();
                deltaParticles.gameObject.SetActive(false);
                deltaParticles2.Play();
                dialogueManager.DisplayDialogues(DialogueConstants.IntroScene.PartSix.dialogues);
                memoryFieldView.gameObject.SetActive(false);
                memoryFileView.gameObject.SetActive(true);
                MoveParallaxContainer(memoryFileView, DialogueConstants.IntroScene.PartSix.dialogues, IntroSceneConstants.screenFadeTime);
                screenFader.StartFadeWithTime(FadeType.FadeOut, IntroSceneConstants.screenFadeTime);
                break;
            case IntroSceneState.PartSeven:
                screenFader.StartFadeWithTime(FadeType.FadeIn, IntroSceneConstants.screenFadeTime);
                break;
            case IntroSceneState.PartEight:
                startTime = Time.time;
                deltaParticles2.gameObject.SetActive(false);
                sideParticlesBack.Play();
                sideParticlesFront.Play();
                dialogueManager.DisplayDialogues(DialogueConstants.IntroScene.PartEight.dialogues);
                memoryFileView.gameObject.SetActive(false);
                memoryElevatorView.gameObject.SetActive(true);
                screenFader.StartFadeWithTime(FadeType.FadeOut, IntroSceneConstants.screenFadeTime);
                break;
            case IntroSceneState.PartNine:
                screenFader.StartFadeWithTime(FadeType.FadeIn, IntroSceneConstants.screenFadeTime);
                break;
            case IntroSceneState.PartTen:
                mainCamera.orthographicSize = IntroSceneConstants.cameraDefaultSize;
                mainCamera.transform.position = new Vector3(0.0f, 0.0f, IntroSceneConstants.cameraZPos);
                sideParticlesBack.Stop();
                sideParticlesBack.gameObject.SetActive(false);
                sideParticlesFront.Stop();
                sideParticlesFront.gameObject.SetActive(false);
                dialogueManager.DisplayDialogues(DialogueConstants.IntroScene.PartTen.dialogues);
                memoryElevatorView.gameObject.SetActive(false);
                memoryExperiencesView.gameObject.SetActive(true);
                MoveParallaxContainer(memoryExperiencesView, DialogueConstants.IntroScene.PartSix.dialogues, IntroSceneConstants.screenFadeTime);
                screenFader.StartFadeWithTime(FadeType.FadeOut, IntroSceneConstants.screenFadeTime);
                break;
            case IntroSceneState.PartEleven:
                screenFader.StartFadeWithTime(FadeType.FadeIn, IntroSceneConstants.screenFadeTime);
                break;
            case IntroSceneState.PartTwelve:
                memoryExperiencesView.gameObject.SetActive(false);
                screenFader.StartFadeWithTime(FadeType.FadeOut, IntroSceneConstants.screenFadeTime);
                videoPlayer.transform.parent.gameObject.SetActive(true);
                dialogueManager.DisplayDialogues(DialogueConstants.IntroScene.PartTwelve.dialogues);
                videoPlayer.Play();
                break;
            case IntroSceneState.PartThirteen:
                videoPlayer.Stop();
                videoPlayer.transform.parent.gameObject.SetActive(false);
                introImageSad.gameObject.SetActive(true);
                dialogueManager.DisplayDialogues(DialogueConstants.IntroScene.PartThirteen.dialogues);
                dialogueManager.SetDialogueColor(Color.red);
                break;
        }
    }

    private void MoveParallaxContainer(Transform transform, Dialogue[] dialogues, float extraTime = 0.0f) {
        Parallax container = transform.GetComponent<Parallax>();
        container.SetMovementTime(
            DialogueConstants.TotalDialogueTime(
                dialogues,
                DialogueManager.DialogueManagerConstants.dialogueFadeTime + extraTime
            )
        );
        container.SetEasingFunction(EasingFuncs.EaseOut);
        container.Move();
    }

    IEnumerator PlayIntroVideo() {
        yield return new WaitForSeconds(IntroSceneConstants.screenFadeDelay);        
        videoPlayer.Play();
    }

    IEnumerator DelayThenDisplayDialogues(Dialogue[] dialogues, float delayTime) {
        yield return new WaitForSeconds(delayTime);
        dialogueManager.DisplayDialogues(dialogues);
    }
}

public enum IntroSceneState {
    NotStarted,
    PartOne, // Zooming into protagonist
    PartTwo, // Scrolling up and fading out screen
    PartThree, // Fading out and showing memory: Field, then starting dialogue
    PartFour, // Displaying dialogue, then starting to fade out when Dialogue is done
    PartFive, // Faded out then displaying the File View when the screen fades back in
    PartSix, // Displaying FileView and Dialogue
    PartSeven, // Fading out and showing the side view
    PartEight, // Fading in and Displaying Side View and Dialogue,
    PartNine, // Fading out and showing the Experiences View
    PartTen, // Fading in and Displaying Experiences View and Dialogue
    PartEleven, // Fading out and showing the protagonist
    PartTwelve, // Fading in and Displaying protagonist and Dialogue
    PartThirteen // Zooming into the prtoganist and displaying dialogue
}