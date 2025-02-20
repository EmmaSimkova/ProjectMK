using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    
    [Header("---------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip jump;
    public AudioClip death;
    public AudioClip hpDamage;
    public AudioClip footsteps;
    public AudioClip crystalCollect;
    public AudioClip depozit;
    public AudioClip crystalDamage;
    public AudioClip crystalBreak;


    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
