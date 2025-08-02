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
        
        SceneManager.LoadScene("level1");
    }

    public void OnlineMode()
    {
      
        SceneManager.LoadScene("LoginMenu");
    }
}