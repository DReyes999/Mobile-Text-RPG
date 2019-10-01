using UnityEngine.Audio;
using UnityEngine;
using System;

/*
 * Purpose of this class is to create a repository of different audio events.
 * We can add and remove them as we go and each event has different properties.
 */


public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }
    public Sound[] sounds;

	// Use this for initialization
	void Awake ()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
        /* Loop through all sounds and an an audio source */

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
	}

    void Start()
    {
        FindObjectOfType<AudioManager>().PlaySound("titleScreenMusic");
    }

    public void PlaySound(string eventName)
    {
        Sound soundToPlay = Array.Find(sounds, sound => sound.eventName == eventName);
        soundToPlay.source.Play();
    }
}
