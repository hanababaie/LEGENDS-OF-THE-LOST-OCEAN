using UnityEngine;
using System;
using System.IO;

[Serializable]
public class LevelSaveData
{
    public int unlockedLevel;
}

public static class LevelSaveJSON
{
    private static readonly string savePath = Application.persistentDataPath + "/levelSave.json";

    public static void Save(int unlockedLevel)
    {
        LevelSaveData data = new LevelSaveData { unlockedLevel = unlockedLevel };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Level saved to JSON: " + savePath);
    }

    public static int Load(int defaultLevel = 1)
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            LevelSaveData data = JsonUtility.FromJson<LevelSaveData>(json);
            return data.unlockedLevel;
        }
        return defaultLevel;
    }
}
