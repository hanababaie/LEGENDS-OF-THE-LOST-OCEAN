using UnityEngine;
using UnityEngine.SceneManagement;

public class bottons : MonoBehaviour

{
    public GameObject mainpanel;
    public GameObject levelmenu;

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
        mainpanel.SetActive(false);
        levelmenu.SetActive(true);
    }

    public void OnlineMode()
    {

        SceneManager.LoadScene("LoginMenu");
    }

    public void backtomain()
    {
        mainpanel.SetActive(true);
        levelmenu.SetActive(false);
        SceneManager.LoadScene("mianmenu");
    }
}