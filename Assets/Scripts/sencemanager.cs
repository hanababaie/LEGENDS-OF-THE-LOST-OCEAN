using UnityEngine;
using UnityEngine.SceneManagement;

public class sencemanager : MonoBehaviour
{
    public static sencemanager Instance;

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

        if (player1.currentlives <= 0 || player2.currentlives <= 0)
        {
            GameOver();
        }
    }

    public void ready()
    {
        if (player1.haskey && player1.atship && player2.haskey && player2.atship)
        {
            LoadNextLevel();
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
        SceneManager.LoadScene("level2");
    }
}
