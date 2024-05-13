using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource mainSong;
    public AudioSource loopedSong;
    public bool startScenePlaying = true;
    public MusicState state = MusicState.NotStarted;
    
    public float musicFadeTime = Constants.targetFPS * 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        if (startScenePlaying)
        {
            StartSongs();
        }
        StartCoroutine(WaitForSongEnd());
    }

    public void StartSongs()
    {
        mainSong.Play();
        mainSong.volume = 1;

        loopedSong.volume = 0;
        loopedSong.Play();
        state = MusicState.Playing;
    }

    IEnumerator WaitForSongEnd()
    {
        yield return new WaitForSeconds(loopedSong.clip.length);
        state = MusicState.SwappingSongs;
    }

    public void Update() {
        if (state == MusicState.SwappingSongs) {
            SwapSongs();
        }
    }
    
    public void SwapSongs() {
        mainSong.volume -= Time.deltaTime / musicFadeTime;
        loopedSong.volume += Time.deltaTime / musicFadeTime;
        if (mainSong.volume <= 0) {
            mainSong.Stop();
            loopedSong.volume = 1;
            state = MusicState.PlayingLooped;
        }
    
    }
}

public enum MusicState
{
    NotStarted,
    Playing,
    SwappingSongs, // Used when slowly increasing the volume of the looped song while decreasing the volume of the main song
    PlayingLooped
}