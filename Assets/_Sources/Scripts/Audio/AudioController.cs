using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [SerializeField] private AudioSource sourceMusic;
    [SerializeField] private AudioSource sourceSFX;
    [SerializeField] private AudioClip menuMusic;

    public float MusicVolume
    {
        get { return PlayerPrefs.GetFloat("MusicPref", 0.05f); }
        set { PlayerPrefs.SetFloat("MusicPref", value); }
    }

    public float SFXVolume
    {
        get { return PlayerPrefs.GetFloat("SFXPref", 0.05f); }
        set { PlayerPrefs.SetFloat("SFXPref", value); }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        sourceMusic.volume = MusicVolume;
        sourceSFX.volume = SFXVolume;
    }

    void Start()
    {
        PlayMusic(menuMusic);
    }

    public static void PlaySFX(AudioClip sfx)
    {
        Instance.sourceSFX.PlayOneShot(sfx);
    }

    public void PlayMusic(AudioClip music)
    {
        sourceMusic.Stop();
        sourceMusic.clip = music;
        sourceMusic.Play();
    }

    public void StopMusic()
    {
        sourceMusic.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        sourceMusic.volume = volume;
        MusicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sourceSFX.volume = volume;
        SFXVolume = volume;
    }
}
