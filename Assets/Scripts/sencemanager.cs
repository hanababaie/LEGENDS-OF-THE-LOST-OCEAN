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

    public int unloacked = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            loadgameState();

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void savedgame()
    {
                Debug.Log("Saving game...");

        gamedata data = new gamedata()
        {
            lastunlockedlevel = LevelMenu.GetUnlockedLevel(),
            currentscene = SceneManager.GetActiveScene().name,
            isLevel3 = player1.isLevel3 || player2.isLevel3,
            player1Data = new playerdata()
            {
                health = player1.currentHealth,
                lives = player1.currentlives,
                coins = player1.coins,
                hasKey = player1.haskey,
                hasKey2 = player1.haskey2,
                atShip = player1.atship,
                atFinalDoor = player1.atfinaldoor
            },
            player2Data = new playerdata()
            {
                health = player2.currentHealth,
                lives = player2.currentlives,
                coins = player2.coins,
                hasKey = player2.haskey,
                atShip = player2.atship,
                atFinalDoor = player2.atfinaldoor
            }
        };

        savingsystem.savinGame(data);
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

    public void ready()
    {
        if ((player1.haskey || player2.haskey) && player1.atship && player2.atship)
        {
            LevelCompleted(1);
            SceneManager.LoadScene("level2");
        }
    }

    public void ready2()
    {
        if (player2.finalkey && player1.atfinaldoor && player2.atfinaldoor)
        {
            LevelCompleted(2);
            SceneManager.LoadScene("level3");

        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("Game Over!");
        savedgame();

        SceneManager.LoadScene("gameover");
    }



    public void LevelCompleted(int index)
    {

        LevelMenu.UnlockNextLevel(index);
        savedgame();
    }

    public void loadgameState()
    {
        gamedata savedData = savingsystem.loadingGame();
        player1.LoadPlayerData(savedData.player1Data);
        player2.LoadPlayerData(savedData.player2Data);
    }

}
