using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemanageronline : MonoBehaviour
{
    public static scenemanageronline Instance;

    private playermovement1 player1;
     private  playermovement2 player2;

    private bool isGameOver = false;
    public bool isloading = false;
    public bool startAtSpawn = false;

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

        // اگه پلیرها هنوز Spawn نشدن صبر کن
        if (player1 == null || player2 == null)
        {
            TryFindPlayers();
            return;
        }

        // چک کردن گیم‌اور
        if (player1.isLevel3)
        {
            if (player1.currentHealth <= 0 && player2.currentlives <= 0)
                GameOver();
        }
        else
        {
            if (player1.currentlives <= 0 || player2.currentlives <= 0)
                GameOver();
        }

        // چک کردن پیشرفت مرحله
        CheckLevelProgression();
    }

    private void TryFindPlayers()
    {
        if (player1 == null) player1 = FindObjectOfType<playermovement1>();
        if (player2 == null) player2 = FindObjectOfType<playermovement2>();
    }

    private void CheckLevelProgression()
    {
        if ((player1.haskey || player2.haskey) && player1.atship && player2.atship)
        {
            isloading = true;
            startAtSpawn = true;
            StartCoroutine(LoadSceneDelayed("level2"));
        }

        if (player2.finalkey && player1.atfinaldoor && player2.atfinaldoor)
        {
            isloading = true;
            startAtSpawn = true;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // وقتی صحنه لود شد دوباره دنبال پلیرها بگرد
        TryFindPlayers();
        ResetLevelFlags();
        startAtSpawn = false;
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
        isGameOver = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnApplicationQuit()
    {
        // کوین‌ها ریست بشن
        if (player1 != null) player1.coins = 0;
        if (player2 != null) player2.coins = 0;
    }
}
