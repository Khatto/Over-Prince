using Unity.VisualScripting;
using UnityEngine;

public struct Dialogue {
    public string text;
    public string audioClipPath;
    //AudioClip audioClip = Resources.Load <AudioClip> ("Music/Song_Name"); 
    public float displayTime;

    public Choice[] choices;

    public Dialogue(string text, string audioClipPath, float displayTime = 0.0f, Choice[] choices = null) {
        this.text = text;
        this.audioClipPath = audioClipPath;
        this.displayTime = displayTime;
        this.choices = choices;
    }

    public bool IsTimed() {
        return displayTime > 0.0f;
    }

    public bool IsChoice() {
        return choices != null;
    }
}