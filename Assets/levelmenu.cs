using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelmenu : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        int unlockedlevel = PlayerPrefs.GetInt("unlockedlevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedlevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void openlevel(int level)
    {
        String levelname = "level" + level;
        SceneManager.LoadScene(levelname);
    }
}
