using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioClip menuMusic;
    public AudioClip level1Music;
    public AudioClip level2Music;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject); // نابود کردن نسخه‌های اضافی
        }
    }

    void Start()
    {
        // اجرای دستی موسیقی هنگام شروع اولین صحنه
        string currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case "mianmenu":
                PlayMusic(menuMusic);
                break;
            case "level1":
                PlayMusic(level1Music);
                break;
            case "level2":
                PlayMusic(level2Music);
                break;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        Debug.Log("Current Scene: " + scene.name);///رای تشخیص اسم صحنه
        switch (scene.name)
        {
            case "mianmenu":
                PlayMusic(menuMusic);
                break;
            case "level1":
                PlayMusic(level1Music);
                break;
            case "level2":
                PlayMusic(level2Music);
                break;
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void ToggleMute()
    {
        audioSource.mute = !audioSource.mute;
    }
}