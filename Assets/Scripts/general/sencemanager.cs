using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sencemanager : MonoBehaviour
{
    public static sencemanager Instance;

    public bool startatspawn = false;

    public p1offline player1;
    public p2offline player2;
    public ChunkGenerator chunkGenerator;

    private bool isGameOver = false;

    public bool startAtSpawn = false;

    public bool isloading = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            player1 = FindObjectOfType<p1offline>();
            player2 = FindObjectOfType<p2offline>();
            chunkGenerator = FindObjectOfType<ChunkGenerator>();

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
        if (player1 != null && player2 != null)
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
        if (player1 == null || player2 == null) return;
        if ((player1.haskey || player2.haskey) && player1.atship && player2.atship)
        {
            isloading = true;
            ClearChunkSequence();
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
        player1.ResetLevelStats();
        player2.ResetLevelStats();
        startatspawn = true;
        ClearChunkSequence();
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
            player2Data = player2.GetPlayerData(),
            chunkSequence = chunkGenerator != null ? chunkGenerator.GetChunkSequence() : SaveManager.LoadGame().chunkSequence,


        };

        if (SceneManager.GetActiveScene().name == "level3" && Camera.main != null)
        {
            data.cameraX = Camera.main.transform.position.x;
            data.cameraY = Camera.main.transform.position.y;
            data.cameraZ = Camera.main.transform.position.z;
        }

        SaveManager.SaveGame(data);
    }

    public void LoadGameState()
    {
        if (!SaveManager.SaveExists()) return;
        GameData data = SaveManager.LoadGame();

        if (startatspawn || LevelMenu.startAtSpawn)
        {
            data.player1Data.posX = player1.spawnPoint.position.x;
            data.player1Data.posY = player1.spawnPoint.position.y;
            data.player1Data.posZ = player1.spawnPoint.position.z;
            data.player2Data.posX = player2.spawnPoint.position.x;
            data.player2Data.posY = player2.spawnPoint.position.y;
            data.player2Data.posZ = player2.spawnPoint.position.z;
            ResetLevelFlags();
        }

        if ((startatspawn || LevelMenu.startAtSpawn) &&
    SceneManager.GetActiveScene().name == "level3" && Camera.main != null)
        {
            Camera.main.transform.position = new Vector3(4450, -800, -10);
        }
        if (!(startatspawn || LevelMenu.startAtSpawn) &&
    SceneManager.GetActiveScene().name == "level3" && Camera.main != null)
        {
            Camera.main.transform.position = new Vector3(data.cameraX, data.cameraY, data.cameraZ);
        }



        player1.LoadPlayerData(data.player1Data);
        player2.LoadPlayerData(data.player2Data);

        if (chunkGenerator != null)
        {
            if (data.chunkSequence != null && data.chunkSequence.Count > 0)
            {
                chunkGenerator.SetChunkSequence(data.chunkSequence);
            }
            else
            {
                chunkGenerator.SetChunkSequence(new System.Collections.Generic.List<int>());
            }
        }


    }

    private void ClearChunkSequence()
    {
        if (chunkGenerator != null)
        {
            chunkGenerator.ClearChunks();

            GameData data = SaveManager.LoadGame();
            data.chunkSequence = new System.Collections.Generic.List<int>();
            SaveManager.SaveGame(data);

            Debug.Log("Chunk sequence cleared after finishing Level 1");
        }
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
        player1 = FindObjectOfType<p1offline>();
        player2 = FindObjectOfType<p2offline>();
        chunkGenerator = FindObjectOfType<ChunkGenerator>();

        if (player1 != null && player2 != null && chunkGenerator != null)
        {
            LoadGameState();
            ResetLevelFlags();

            if (scene.name == "level1" && chunkGenerator != null)
            {
                chunkGenerator.GenerateChunksAtStart(chunkGenerator.startp1);
            }
            LevelMenu.startAtSpawn = false;
            startatspawn = false;
        }
    }

    public void ResetLevelFlags()
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


        SaveGame();
    }

    public void ResetPlayerPositions()
    {
        if (player1 != null && player1.spawnPoint != null)
        {
            player1.transform.position = player1.spawnPoint.position;
        }
        if (player2 != null && player2.spawnPoint != null)
        {
            player2.transform.position = player2.spawnPoint.position;
        }
    }

    public void Resetafterfinisfh()
    {
        if (player1 != null)
        {
            player1.haskey = false;
            player1.atship = false;
            player1.atfinaldoor = false;
            player1.currentHealth = player1.maxHealth;
            player1.currentlives = player1.maxHealth;
        }

        if (player2 != null)
        {
            player2.haskey = false;
            player2.atship = false;
            player2.atfinaldoor = false;
            player2.finalkey = false;
            player2.currentHealth = player2.maxHealth;
            player2.currentlives = player2.maxHealth;
        }

        isloading = false;

        GameData data = SaveManager.LoadGame();
        data.currentScene = "level1";  // مرحله ۱
        data.player1Data.posX = player1.spawnPoint.position.x;
        data.player1Data.posY = player1.spawnPoint.position.y;
        data.player1Data.posZ = player1.spawnPoint.position.z;
        data.player2Data.posX = player2.spawnPoint.position.x;
        data.player2Data.posY = player2.spawnPoint.position.y;
        data.player2Data.posZ = player2.spawnPoint.position.z;
        SaveManager.SaveGame(data);
    }
}