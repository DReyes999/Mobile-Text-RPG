using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_AudioManager : MonoBehaviour {

    public string startMusic;
    public string startAmbience;

    // Use this for initialization
    void Start()
    {
        if (startMusic !=null)
            AudioEngine.Instance.PlaySound(startMusic);
        if (startAmbience!= null)
            AudioEngine.Instance.PlaySound(startAmbience);

    }

}
