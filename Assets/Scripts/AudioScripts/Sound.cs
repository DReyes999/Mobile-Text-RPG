using System;
using UnityEngine;
using UnityEngine.Audio;

// IF we want this class to show up in the inspector we have to make it serializeable
[System.Serializable]
public class Sound  // Must remove Monobehavior to get this to work like before
{
    public string eventName;
    public AudioClip clip;

    public static explicit operator Sound(UnityEngine.Object v)
    {
       throw new NotImplementedException();
    }

    /* List of parameters for all sounds we add */
    [Range(0f , 1.0f)]
    public float volume = 0.5f;
    [Range(0.1f , 3.0f)]
    public float pitch = 1.0f;

    public bool loop;

    public bool fadeOut;
    public float fadeOutSeconds = 1.0f;

    public bool fadeIn;
    public float fadeInSeconds = 1.0f;

    public bool random;

    public AudioMixerGroup mixerOutput;

    [HideInInspector]
    public AudioSource source;

    public Sound()
    {

    }

    public Sound(string name)
    {
        eventName = name;
    }

}

