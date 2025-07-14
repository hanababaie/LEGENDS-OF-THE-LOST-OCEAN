using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] levelButtons;
    public Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 0.7f); 
    public Color unlockedColor = Color.white;

    private void Start()
    
    {
        // SaveManager.DeleteSave();
        // PlayerPrefs.DeleteAll();

        Debug.Log("UnlockedLevel from PlayerPrefs: " + GetUnlockedLevel());
        int unlockedLevel = GetUnlockedLevel();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = (i + 1) <= unlockedLevel;
        
        }
    }

    public void ResetSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Save data reset done!");
    }

    public void OpenLevel(int level)
    {
        Debug.Log("OpenLevel called with level: " + level);
        int unlockedLevel = GetUnlockedLevel();
        if (level > unlockedLevel)
        {
            Debug.Log("Level is locked");
            return;
        }

        string levelName = "level" + level;
        SceneManager.LoadScene(levelName);
    }

    public static int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt("UnlockedLevel", 1);
    }

    public static void UnlockNextLevel(int currentLevel)
    {
        if (currentLevel >= GetUnlockedLevel())
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
        }
    }
}
