using UnityEngine;
using UnityEngine.SceneManagement;

public class play : MonoBehaviour
{
    public void OnClickContinue()
    {
        if (sencemanager.Instance != null)
        {sencemanager.Instance.startAtSpawn = false;}
        if (!SaveManager.SaveExists())
        {
            Debug.Log("No save file found. Loading level1.");
            SceneManager.LoadScene("level1");
            return;
        }

        GameData data = SaveManager.LoadGame();

        if (!string.IsNullOrEmpty(data.currentScene))
        {
            Debug.Log("Loading saved scene: " + data.currentScene);
            SceneManager.LoadScene(data.currentScene);
        }
        else
        {
            Debug.LogWarning("Saved scene name is empty. Loading level1.");
            SceneManager.LoadScene("level1");
        }
    }

    public void shopopener()
    {
        
        SceneManager.LoadScene("shop");
    }
}