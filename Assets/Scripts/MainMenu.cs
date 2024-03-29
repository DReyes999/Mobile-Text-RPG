﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image black;
    public Animator anim;

    public void PlayGame()
    {
        StartCoroutine(Fading());
    }

    public void QuitGame()
    {
        Debug.Log("Game was Quit");
        Application.Quit();
    }

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene("Game");
    }
}
