using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource mainSong;
    public AudioSource loopedSong;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForSongEnd());
    }

    IEnumerator WaitForSongEnd()
    {
        yield return new WaitUntil(() => !mainSong.isPlaying);
        loopedSong.Play();
    }
}
