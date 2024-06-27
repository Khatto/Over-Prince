using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class IntroSceneManager : MonoBehaviour
{
    public Camera mainCamera;
    public MusicManager musicManager;
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
        public const float sceneTransitionTime = 2.0f;
        public const float musicEchoFadeTime = 3.75f;
        public const float screenFlashTime = 0.2f;

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

        public struct PartTwelve {
            public static Vector2 cameraStartPos = new Vector3(0.0f, 1.16f, cameraZPos);
            public static Vector2 cameraEndPos = new Vector4(0.0f, 2.04f, cameraZPos);
            public const float cameraStartSize = 3.5f;
            public const float cameraEndSize = 2.5f;
        }
    }

    void Start()
    {
        PerformPart(IntroSceneState.PartOne);
    }

    void Update() {
        //Debug.Log("mainCamera.orthoSize: " + mainCamera.orthographicSize + " at " + Time.time + " Camera End Size: " + IntroSceneConstants.PartOne.cameraEndSize + " State: " + state);
        switch (state) {
            case IntroSceneState.PartOne:
                mainCamera.orthographicSize = Mathf.Lerp(
                    IntroSceneConstants.PartOne.cameraStartSize,
                    IntroSceneConstants.PartOne.cameraEndSize, 
                    EasingFuncs.EaseInOut((Time.time - startTime) / IntroSceneConstants.PartOne.cameraZoomTime)
                );
                //if (mainCamera.orthographicSize >= IntroSceneConstants.PartOne.cameraEndSize) {
                if (Time.time - startTime >= IntroSceneConstants.PartOne.cameraZoomTime) {
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
                if (dialogueManager.state == DialogueState.Finished) {
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
                float dialogueTime = DialogueConstants.TotalDialogueTime(
                    DialogueConstants.IntroScene.PartTwelve.dialogues,
                    DialogueManager.DialogueManagerConstants.dialogueFadeTime + IntroSceneConstants.screenFadeTime
                );
                mainCamera.orthographicSize = Mathf.Lerp(
                    IntroSceneConstants.PartTwelve.cameraStartSize,
                    IntroSceneConstants.PartTwelve.cameraEndSize, 
                    EasingFuncs.EaseInOut((Time.time - startTime) / dialogueTime)
                );
                Vector3 cameraPos = Vector3.Lerp(
                    IntroSceneConstants.PartTwelve.cameraStartPos,
                    IntroSceneConstants.PartTwelve.cameraEndPos,
                    EasingFuncs.EaseInOut((Time.time - startTime) / dialogueTime)
                );
                cameraPos.z = IntroSceneConstants.cameraZPos;
                mainCamera.transform.position = cameraPos;

                if (dialogueManager.state == DialogueState.Finished) {
                    PerformPart(IntroSceneState.PartThirteen);
                }
                break;
            case IntroSceneState.PartThirteen:
                if (dialogueManager.state == DialogueState.Finished) {
                    PerformPart(IntroSceneState.PartFourteen);
                }
                break;
        }
    }

    /**
     * Perform the actions for the given state once when the state is initially set
     */
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
                MoveParallaxContainer(memoryElevatorView, DialogueConstants.IntroScene.PartEight.dialogues, IntroSceneConstants.screenFadeTime);
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
                startTime = Time.time;
                mainCamera.orthographicSize = IntroSceneConstants.PartTwelve.cameraStartSize;
                mainCamera.transform.position = IntroSceneConstants.PartTwelve.cameraStartPos;
                memoryExperiencesView.gameObject.SetActive(false);
                screenFader.StartFadeWithTime(FadeType.FadeOut, IntroSceneConstants.screenFadeTime);
                videoPlayer.transform.parent.gameObject.SetActive(true);
                dialogueManager.DisplayDialogues(DialogueConstants.IntroScene.PartTwelve.dialogues);
                videoPlayer.Play();
                break;
            case IntroSceneState.PartThirteen:
                musicManager.FadeOutWithEcho(IntroSceneConstants.musicEchoFadeTime);
                videoPlayer.Stop();
                videoPlayer.transform.parent.gameObject.SetActive(false);
                introImageSad.gameObject.SetActive(true);
                screenFader.StartFadeWithTime(FadeType.FlashInThenFadeOut, IntroSceneConstants.screenFlashTime);
                dialogueManager.DisplayDialogues(DialogueConstants.IntroScene.PartThirteen.dialogues);
                break;
            case IntroSceneState.PartFourteen:
                screenFader.StartFadeWithTime(FadeType.FadeIn, IntroSceneConstants.screenFadeTime);
                StartCoroutine(FadeToNextScene(IntroSceneConstants.sceneTransitionTime));
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

    IEnumerator FadeToNextScene(float delayTime) {
        yield return new WaitForSeconds(delayTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene("IntroStageScene");
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
    PartThirteen, // Zooming into the prtoganist and displaying dialogue,
    PartFourteen // Fading and transitioning to the next scene
}