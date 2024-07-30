using System.Collections;
using UnityEngine;

public class MusicInfoManager : MonoBehaviour
{
    public ShadowedText musicInfoText;
    public MusicInfoState state = MusicInfoState.NotStarted;

    public float musicInfoFadeTime = 0.5f;
    public float musicInfoTotalDisplayTime = 2.0f;
    public Vector2 musicInfoDefaultPosition = new Vector2(0.0f, 0.0f);
    public Vector2 musicInfoSlideOffset = new Vector2(-1.0f, 0.0f);
    public Constants.Song song;

    private void Start()
    {
        musicInfoText.SetupShadowText();
    }

    public void DisplayMusicInfo()
    {
        DisplayMusicInfo(Constants.SongDatas.GetSongData(song));
    }

    public void DisplayMusicInfo(SongData songData)
    {
        musicInfoText.SetText($"♫ \"{songData.title}\" - {songData.artist}");
        StartCoroutine(AnimateMusicInfo());
    }

    private IEnumerator AnimateMusicInfo()
    {
        state = MusicInfoState.FadingIn;
        musicInfoText.StartFadeWithTime(FadeType.FadeIn, musicInfoFadeTime);
        musicInfoText.SetMovementValues(musicInfoTotalDisplayTime, musicInfoSlideOffset, EasingFuncs.Linear);
        musicInfoText.Move();
        yield return new WaitForSeconds(musicInfoFadeTime + (musicInfoTotalDisplayTime - musicInfoFadeTime * 2.0f));
        
        state = MusicInfoState.FadingOut;
        musicInfoText.StartFadeWithTime(FadeType.FadeOut, musicInfoFadeTime);
        yield return new WaitForSeconds(musicInfoFadeTime);

        state = MusicInfoState.Finished;
    }
}

public enum MusicInfoState
{
    NotStarted,
    FadingIn,
    FadingOut,
    Finished
}