using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen_AudioManager : MonoBehaviour {

    public string startMusic;

	// Use this for initialization
	void Start ()
    {
        AudioEngine.Instance.PlaySound(startMusic);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
