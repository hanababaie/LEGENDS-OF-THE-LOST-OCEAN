using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pausemenu : MonoBehaviour
{

    public playermovement1 player1;
    public playermovement2 player2;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Image muteButtonIcon;
    [SerializeField] Sprite soundOnIcon;
    [SerializeField] Sprite soundOffIcon;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause();
        }

    }
    public void pause()
    {
        pauseMenu.SetActive(true);
        Debug.Log("setting");
        Time.timeScale = 0;
    }
    public void mainmenu()
    {
        Time.timeScale = 1;
        SaveGameDirectly();
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
    
     private void SaveGameDirectly()
    {
        gamedata data = new gamedata()
        {
            lastunlockedlevel = LevelMenu.GetUnlockedLevel(), 
            currentscene = SceneManager.GetActiveScene().name,
            isLevel3 = player1.isLevel3 || player2.isLevel3,
            player1Data = new playerdata()
            {
                health = player1.currentHealth,
                lives = player1.currentlives,
                coins = player1.coins,
                hasKey = player1.haskey,
                hasKey2 = player1.haskey2,
                atShip = player1.atship,
                atFinalDoor = player1.atfinaldoor
            },
            player2Data = new playerdata()
            {
                health = player2.currentHealth,
                lives = player2.currentlives,
                coins = player2.coins,
                hasKey = player2.haskey,
                hasKey2 = player2.finalkey,
                atShip = player2.atship,
                atFinalDoor = player2.atfinaldoor
            }
        };

        savingsystem.savinGame(data);
        Debug.Log("Game saved directly from PauseMenu.");
    }
}

