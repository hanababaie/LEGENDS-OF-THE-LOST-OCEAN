using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class sencemanager : MonoBehaviour
{
    public static sencemanager Instance; // a singlton

    public playermovement1 player1;
    public playermovement2 player2;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (isGameOver) return;

        ready();
        ready2();
        if (player1.currentlives <= 0 || player2.currentlives <= 0) // check if we are lost or not
        {
            GameOver();
        }
    }

    public void ready() // for level on
    {
        if (player1.haskey && player1.atship && player2.haskey && player2.atship)
        {
            LoadNextLevel();
        }
    }

    public void ready2() // for level  two
    {
        if (player2.finalkey && player1.atfinaldoor && player2.atfinaldoor)
        {
            Debug.Log("next");
            SceneManager.LoadScene("nextlevel");
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("Game Over!");

        SceneManager.LoadScene("gameover");
    }

     public void LoadNextLevel()
    {
        StartCoroutine(loading("level2", 5f)); // show level 2 after 5 sec
    }

    private IEnumerator loading(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay); // wait 5 sec 
        SceneManager.LoadScene(sceneName);
    }
}
