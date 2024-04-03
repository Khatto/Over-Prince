using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TitleScreenManager : MonoBehaviour
{
    public TextMeshProUGUI startLabel;
    public Fade copyrightInfoFade;
    public VideoPlayer videoPlayer;
    public SpriteRenderer titleImage;
    public Fade screenFlasher;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayVideo());
        StartCoroutine(DelayTextDisplay());
    }

    IEnumerator PlayVideo() {
        videoPlayer.loopPointReached += EndReached;
        yield return new WaitForSeconds(1.2f);
        videoPlayer.Play();
    }

    void EndReached(VideoPlayer videoPlayer)
    {
        videoPlayer.gameObject.active = false;
        titleImage.gameObject.active = true;
        screenFlasher.gameObject.active = true;
        screenFlasher.StartFade(FadeType.FlashInThenFadeOut);
    }

    IEnumerator DelayTextDisplay() {
        yield return new WaitForSeconds(3.8f);
        startLabel.GetComponent<Fade>().StartFade(FadeType.FadeInFadeOut);
        copyrightInfoFade.StartFade(FadeType.FadeIn);
    }
}
