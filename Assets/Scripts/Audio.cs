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
}
