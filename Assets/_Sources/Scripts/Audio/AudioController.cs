using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource sourceMusic;
    [SerializeField] private AudioSource sourceSFX;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    private float MusicVolume
    {
        get { return PlayerPrefs.GetFloat("MusicPref", 0.05f); }
        set { PlayerPrefs.SetFloat("MusicPref", value); }
    }

    private float SFXVolume
    {
        get { return PlayerPrefs.GetFloat("SFXPref", 0.05f); }
        set { PlayerPrefs.SetFloat("SFXPref", value); }
    }

    void Start()
    {
        // PlayMusic(menuMusic);
    }

    public void PlaySFX(AudioClip sfx)
    {
        sourceSFX.PlayOneShot(sfx);
    }

    public void PlayMusic(AudioClip music)
    {
        sourceMusic.Stop();
        sourceMusic.clip = music;
        sourceMusic.Play();
    }
}
