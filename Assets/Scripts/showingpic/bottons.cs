using UnityEngine;
using UnityEngine.SceneManagement;

public class bottons : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("level1"); 
    }

    public void options()
    {
        Debug.Log("Options Clicked");
    }

    public void exit()
    {
        Debug.Log("Exit Clicked");
        Application.Quit();
    }

    public void OfflineMode()
    {
        GameMode.isOnline = false; // مشخص می‌کنیم بازی آفلاینه
        SceneManager.LoadScene("level1");
    }

    public void OnlineMode()
    {
        GameMode.isOnline = true; // مشخص می‌کنیم بازی آنلاین است
        SceneManager.LoadScene("LoginMenu");
    }
}