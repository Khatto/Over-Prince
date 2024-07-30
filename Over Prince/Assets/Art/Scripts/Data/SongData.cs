using System;
using UnityEngine;

/// <summary>
/// Contains the data for a song, incuding the title, artist, bpm, and length
/// </summary>
[Serializable]
public struct SongData {
    public string title;
    public string artist;
    public int bpm;
    public float length;
    public SongData(String title, String artist, int bpm = 0, float length = 0.0f) {
        this.title = title;
        this.artist = artist;
        this.bpm = bpm;
        this.length = length;
    }
}