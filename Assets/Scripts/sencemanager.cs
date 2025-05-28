using UnityEngine;
using UnityEngine.SceneManagement;

public class sencemanager : MonoBehaviour
{
    public static sencemanager Instance;

    public playermovement1 player1;
    public playermovement2 player2;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Update()
    {
        ready();
    }

    public void ready()
    {
        if (player1.haskey && player1.atship && player2.haskey && player2.atship)
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        
        SceneManager.LoadScene("level2");
    }
}
