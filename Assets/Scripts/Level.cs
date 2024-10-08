﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 2f;


    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad());   
    }

    public void LoadGameWin()
    {
        StartCoroutine(WaitAndLoadWin());
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("Game Over");
    }
    IEnumerator WaitAndLoadWin()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("Game Win");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
