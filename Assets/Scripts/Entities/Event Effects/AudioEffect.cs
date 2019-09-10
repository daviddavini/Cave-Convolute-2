using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class AudioEffect : EventEffect
{
    public AudioSource audioSource;
    public float volumeOffsetRange;
    public float pitchOffsetRange;
    private float baseVolume;
    private float basePitch;
    public bool allowOverlap = false;

    void Start()
    {
        baseVolume = audioSource.volume;
        basePitch = audioSource.pitch;
    }

    protected override void DoEffect()
    {
        if (allowOverlap || !audioSource.isPlaying)
        {
            audioSource.volume = baseVolume + volumeOffsetRange*Random.Range(-1f, 1);
            audioSource.pitch = basePitch + pitchOffsetRange*Random.Range(-1f, 1);
            audioSource.PlayOneShot(audioSource.clip);
        }
        // else if(allowOverlap)
        // {
        //     audioSource.PlayOneShot(overlappingAudioClip);
        // }
    }
}
