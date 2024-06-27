using System.Collections;
using UnityEngine;

using TMPro;

public class MusicInfoManager : MonoBehaviour
{
    public ShadowedText musicInfoText;
    private Fade musicInfoFade;
    public MusicInfoState state = MusicInfoState.NotStarted;

    public float musicInfoFadeTime = 0.5f;
    public float musicInfoTotalDisplayTime = 2.0f;
    public Vector2 musicInfoDefaultPosition = new Vector2(0.0f, 0.0f);
    public Vector2 musicInfoSlideOffset = new Vector2(-1.0f, 0.0f);

    private void Start()
    {
        musicInfoText.SetupShadowText();
    }

    public void DisplayMusicInfo(string musicInfo)
    {
        musicInfoText.text = musicInfo;
        StartCoroutine(AnimateMusicInfo());
    }

    private IEnumerator AnimateMusicInfo()
    {
        musicInfoFade.StartFadeWithTime(FadeType.FadeIn, musicInfoFadeTime);
        yield return new WaitForSeconds(musicInfoFadeTime);
        /*
        // Fade in
        fade.FadeIn(musicInfoText, duration: 1f);
        yield return new WaitForSeconds(1f);

        // Move slightly to the left
        Vector3 targetPosition = musicInfoText.transform.position + new Vector3(-1f, 0f, 0f);
        float moveDuration = 0.5f;
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            musicInfoText.transform.position = Vector3.Lerp(musicInfoText.transform.position, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fade out
        fade.FadeOut(musicInfoText, duration: 1f);
        */
        yield return new WaitForSeconds(1f);
    }
}

public enum MusicInfoState
{
    NotStarted,
    FadingIn,
    FadingOut
}