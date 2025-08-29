using UnityEditor;
using UnityEngine;

public class ExportPlayerPrefs
{
    [MenuItem("Tools/Copy PlayerPrefs To Build")]
    public static void CopyUnlockedLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        Debug.Log("Exporting UnlockedLevel: " + unlockedLevel);

        // این PlayerPrefs در بیلد همون مقدار رو خواهد داشت
        PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs exported for build.");
    }
}
