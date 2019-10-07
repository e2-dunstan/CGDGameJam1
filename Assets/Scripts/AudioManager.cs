using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource thisAudioSource;

    public enum ClipType
    {
        DEATH, JUMP, WEB, FIRE, IMPACT, POINTS_GAINED, TAKE_DAMAGE
    }

    public AudioClip[] deathSounds;
    public AudioClip[] impactSounds;
    public AudioClip[] webSounds;
    public AudioClip[] jumpSounds;
    public AudioClip[] fireSounds;
    public AudioClip[] pointsGainedSounds;
    public AudioClip[] takeDamageSounds;

    public AudioClip[] other;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        thisAudioSource = GetComponent<AudioSource>();
    }

    public void PlaySpecificClip(AudioClip clip, Transform audioSource)
    {
        AudioSource source = audioSource.GetComponent<AudioSource>();
        if (source == null)
        {
            source = audioSource.gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
        }
        
        source.PlayOneShot(clip);
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
            case ClipType.TAKE_DAMAGE:
                PlayRandom(takeDamageSounds, audioSource);
                break;
        }
    }

    private void PlayRandom(AudioClip[] clips, Transform audioSource)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length - 1)];

        AudioSource source = audioSource.GetComponent<AudioSource>();
        if (source == null)
        {
            source = audioSource.gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
        }
        
        source.PlayOneShot(clip);
    }
}