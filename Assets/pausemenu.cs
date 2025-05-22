using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pausemenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Image muteButtonIcon;
    [SerializeField] Sprite soundOnIcon;
    [SerializeField] Sprite soundOffIcon;

    public void pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void mainmenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("mianmenu");
    }

    public void resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void mute()
    {
            if (AudioListener.volume == 0)
            {
                AudioListener.volume = 1f;
                muteButtonIcon.sprite = soundOnIcon;
                
            }
            else
            {
                AudioListener.volume = 0f;
                muteButtonIcon.sprite = soundOffIcon;
                
            }
    }
}

