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
        {
            Instance = this;
            LoadGameState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        if (player1.currentlives <= 0 || player2.currentlives <= 0)
        {
            GameOver();
        }

        CheckLevelProgression();
    }

    private void CheckLevelProgression()
    {
        if ((player1.haskey || player2.haskey) && player1.atship && player2.atship)
        {
            LevelCompleted(1);
            SceneManager.LoadScene("level2");
        }

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
        SaveGame();
        SceneManager.LoadScene("gameover");
    }

    public void LevelCompleted(int levelIndex)
    {
        int unlocked = Mathf.Max(LevelMenu.GetUnlockedLevel(), levelIndex + 1);
        PlayerPrefs.SetInt("UnlockedLevel", unlocked);
        PlayerPrefs.Save();
        SaveGame();
    }

    public void SaveGame()
    {
        GameData data = new GameData
        {
            lastUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1),
            currentScene = SceneManager.GetActiveScene().name,
            isLevel3 = player1.isLevel3 || player2.isLevel3,
            player1Data = player1.GetPlayerData(),
            player2Data = player2.GetPlayerData()
        };

        SaveManager.SaveGame(data);
    }

    public void LoadGameState()
    {
        if (!SaveManager.SaveExists()) return;

        GameData data = SaveManager.LoadGame();
        player1.LoadPlayerData(data.player1Data);
        player2.LoadPlayerData(data.player2Data);
    }
}
