﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource thisAudioSource;

    public enum ClipType
    {
        DEATH, JUMP, WEB, FIRE, IMPACT, POINTS_GAINED
    }

    public AudioClip[] deathSounds;
    public AudioClip[] impactSounds;
    public AudioClip[] webSounds;
    public AudioClip[] jumpSounds;
    public AudioClip[] fireSounds;
    public AudioClip[] pointsGainedSounds;

    void Awake()
    {
        if (Instance == null) Instance = this;

        thisAudioSource = GetComponent<AudioSource>();
    }
    

    public void PlayRandomClip(ClipType clipType, Transform audioSource)
    {
        switch (clipType)
        {
            case ClipType.DEATH:
                PlayRandom(deathSounds, audioSource);
                break;
            case ClipType.JUMP:
                PlayRandom(jumpSounds, audioSource);
                break;
            case ClipType.WEB:
                PlayRandom(webSounds, audioSource);
                break;
            case ClipType.FIRE:
                PlayRandom(fireSounds, audioSource);
                break;
            case ClipType.IMPACT:
                PlayRandom(impactSounds, audioSource);
                break;
            case ClipType.POINTS_GAINED:
                PlayRandom(pointsGainedSounds, audioSource);
                break;
        }
    }

    private void PlayRandom(AudioClip[] clips, Transform audioSource)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length - 1)];

        AudioSource source = audioSource.GetComponent<AudioSource>();
        if (source == null)
            source = audioSource.gameObject.AddComponent<AudioSource>();

        source.playOnAwake = false;
        //source.clip = clip;
        source.PlayOneShot(clip);
    }
}