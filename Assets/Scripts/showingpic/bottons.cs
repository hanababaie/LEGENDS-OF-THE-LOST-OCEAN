using UnityEngine;
using UnityEngine.SceneManagement;

public class bottons : MonoBehaviour

{
    public GameObject mainpanel;
    public GameObject levelmenu;
    public GameObject optionmenu;

    public void StartGame()
    {
        SceneManager.LoadScene("level1");
    }

    public void options()
    {
        Debug.Log("Options Clicked");
        mainpanel.SetActive(false);
        optionmenu.SetActive(true);
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
        optionmenu.SetActive(false);

    }
    
    public void ResetProgress()
    {
        SaveManager.DeleteSave();
        Debug.Log("Progress has been reset.");
    }
}