using System.Collections;
using System.IO;
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
            DontDestroyOnLoad(gameObject);
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
            ResetLevelFlags();
            StartCoroutine(LoadSceneDelayed("level2"));
        }

        if (player2.finalkey && player1.atfinaldoor && player2.atfinaldoor)
        {
            LevelCompleted(2);
            ResetLevelFlags();
            StartCoroutine(LoadSceneDelayed("level3"));
        }
    }

    private IEnumerator LoadSceneDelayed(string sceneName)
    {
        yield return new WaitForSeconds(1f); // می‌تونی این رو هم حذف کنی یا کمترش کنی
        SceneManager.LoadScene(sceneName);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        player1.ResetLevelStats();
        player2.ResetLevelStats();
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player1 = FindObjectOfType<playermovement1>();
        player2 = FindObjectOfType<playermovement2>();

        if (player1 != null && player2 != null)
        {
            LoadGameState();
            ResetLevelFlags();
        }
    }
    
    private void ResetLevelFlags()
    {
        if (player1 != null)
        {
            player1.haskey = false;
            player1.atship = false;
            player1.atfinaldoor = false;
        }

        if (player2 != null)
        {
            player2.haskey = false;
            player2.atship = false;
            player2.atfinaldoor = false;
            player2.finalkey = false;
        }
    }
}
