using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

[CanEditMultipleObjects]
public class AudioEngine : MonoBehaviour
{
    public static AudioEngine Instance { get; private set; }
    public List<Sound> sounds = new List<Sound>();

    public AudioMixer masterMixer;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);

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
            sound.source.outputAudioMixerGroup = sound.mixerOutput;
        }
    }

    private void Start()
    {
        //PlaySound("StartMusic");
    }

    public void PlaySound(string eventName)
    {
        Sound soundToPlay = sounds.Find(sound => sound.eventName == eventName);
        if(soundToPlay == null)
        {
            Debug.LogWarning(string.Format("could not find event with name: {0}", eventName));
            return;
        }
        soundToPlay.source.Play();

         if (soundToPlay.fadeIn)
        {
            StartCoroutine(SliderFadeIn(soundToPlay.fadeInSeconds, soundToPlay.source,soundToPlay.source.volume));
        }
    }

    public void StopSound(string eventName)
    {
        Sound soundToStop = sounds.Find(sound => sound.eventName == eventName);
        if (soundToStop == null)
        {
            Debug.LogWarning(string.Format("could not find event with name: {0}", eventName));
            return;
        }
        Debug.Log(eventName + "Found. StopSound called");
        if (soundToStop.fadeOut)
            StartCoroutine(SliderFadeOut(soundToStop.fadeOutSeconds, 
            soundToStop.source, soundToStop.source.volume));

        //soundToStop.source.Stop();
    }

    IEnumerator SliderFadeIn(float seconds, AudioSource source, float originalVolume)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            source.volume = Mathf.Lerp(0.0f, originalVolume, lerpValue);
            yield return null;
        }
    }

    IEnumerator SliderFadeOut(float seconds, AudioSource source, float originalVolume)
    {
        Debug.Log("SliderFadeOut Called");
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            //Debug.Log("In the while loop. Seconds = " + seconds);
            //Debug.Log("In the while loop. original volume = " + originalVolume);
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            //Debug.Log("Lerp Value: " + lerpValue);
            source.volume = Mathf.Lerp(originalVolume, 0.0f, lerpValue);
            yield return null;
        }

        source.Stop();
    }
    /*
    Below are Methods for using logarithmic values
    to create a more smooth and natural volume slider
    that correctly manipulates the mixer faders
     */
    public void AdjustMusicVolume( float newVolume)
    {
        masterMixer.SetFloat("musicVol", Mathf.Log10(newVolume) *20);
    }
    public void AdjustSFXVolume( float newVolume)
    {
        masterMixer.SetFloat("sfxVol", Mathf.Log10(newVolume) *20);
    }

}
