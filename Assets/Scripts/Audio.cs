using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    //singleton
    public static Audio Instance;

    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip MenueMusic;
    public AudioClip LevelMusic;

    public AudioClip BigBallBounce;
    public AudioClip BallBounce;
    public AudioClip ExtraLife;
    public AudioClip LostLife;
    public AudioClip SpeedBost;
    public AudioClip GameOver;
    public AudioClip BtnPress;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadVolumeSettings();
    }
    public void MusicSwitch(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }
    public void SFXplayer(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    // Volume control methods
    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        if (MusicSource != null)
        {
            MusicSource.volume = volume;
        }

        // Save the volume setting
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        SFXSource.volume = volume;

        // Save the volume setting
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public float GetMusicVolume()
    {
        if (MusicSource != null)
        {
            return MusicSource.volume;
        }
        return PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public float GetSFXVolume()
    {
        return SFXSource.volume;

        return PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    // Load saved volume settings
    public void LoadVolumeSettings()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);
    }


}

