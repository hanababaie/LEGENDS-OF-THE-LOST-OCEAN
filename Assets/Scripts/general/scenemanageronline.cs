using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemanageronline : MonoBehaviour
{
    public static scenemanageronline  Instance;

    public bool startatspawn = false;

    public playermovement1 player1;
    public playermovement2 player2;

    private bool isGameOver = false;

    public bool startAtSpawn = false;
    public bool isloading = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        if (player1 != null && player2 != null) // ✅ Null-check
        {
            if (player1.isLevel3)
            {
                if (player1.currentHealth <= 0 && player2.currentlives <= 0)
                {
                    GameOver();
                }
            }
            else
            {
                if (player1.currentlives <= 0 || player2.currentlives <= 0)
                {
                    GameOver();
                }
            }

            CheckLevelProgression();
        }
    }

    private void CheckLevelProgression()
    {
        if (player1 == null || player2 == null) return; // ✅ Null-check

        if ((player1.haskey || player2.haskey) && player1.atship && player2.atship)
        {
            isloading = true;
            LevelCompleted(1);
            startatspawn = true;
            StartCoroutine(LoadSceneDelayed("level2"));
        }

        if (player2.finalkey && player1.atfinaldoor && player2.atfinaldoor)
        {
            isloading = true;
            LevelCompleted(2);
            startatspawn = true;
            StartCoroutine(LoadSceneDelayed("level3"));
        }
    }

    private IEnumerator LoadSceneDelayed(string sceneName)
    {
        ResetLevelFlags();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (player1 != null) player1.ResetLevelStats();
        if (player2 != null) player2.ResetLevelStats();

        SceneManager.LoadScene("gameover");
    }

    public void LevelCompleted(int levelIndex)
    {
        int unlocked = Mathf.Max(LevelMenu.GetUnlockedLevel(), levelIndex + 1);
        PlayerPrefs.SetInt("UnlockedLevel", unlocked);
        PlayerPrefs.Save();
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
            ResetLevelFlags();

            LevelMenu.startAtSpawn = false;
            startatspawn = false;
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

        isloading = false;
    }

    private void OnApplicationQuit()
    {
        if (player1 != null) player1.coins = 0;
        if (player2 != null) player2.coins = 0;
    }
}
