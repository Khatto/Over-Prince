using UnityEngine;

public struct Dialogue {
    public string text;
    public string audioClipPath;
    //AudioClip audioClip = Resources.Load <AudioClip> ("Music/Song_Name"); 

    public Dialogue(string text, string audioClipPath) {
        this.text = text;
        this.audioClipPath = audioClipPath;
    }
}