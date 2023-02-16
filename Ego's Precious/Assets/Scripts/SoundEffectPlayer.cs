using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioSource> effects;

    public void PlayDanger1()
    {
        effects[0].Play();
    }

    public void PlayDanger2() 
    {
        effects[1].Play();
    }

    public void PlayTear()
    {
        effects[2].Play();
    }

    public void PlayTearFast()
    {
        effects[3].Play();
    }

    public void PlayTearCardboard()
    {
        effects[4].Play();
    }

    public void PlayBandaid()
    {
        effects[5].Play();
    }

    public void PlayStorm()
    {
        effects[6].Play();
    }

    public void PlayBubble()
    {
        effects[7].Play();
    }

    public void PlaySlide()
    {
        effects[8].Play();
    }
}
