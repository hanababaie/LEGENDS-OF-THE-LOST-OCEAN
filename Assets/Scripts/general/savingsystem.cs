using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;


[Serializable]
public class GameData
{
    public int lastUnlockedLevel;
    public string currentScene;
    public bool isLevel3;

    public PlayerData player1Data;
    public PlayerData player2Data;

    public List<int> chunkSequence;

    public float cameraX;
    public float cameraY;
    public float cameraZ;
}
[Serializable]
public class PlayerData
{
    public int health;
    public int lives;
    public int coins;
    public int totalcoins;
    public bool hasKey;
    public bool hasKey2;
    public bool atShip;
    public bool atFinalDoor;

    public int maxhealth;
    public float maxspeed;
    public int extraLife;

    public float posX;
    public float posY;
    public float posZ;

    
}


public static class SaveManager
{
    private static readonly string savePath = Application.persistentDataPath + "/save.json";

    public static void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public static GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<GameData>(json);
        }

        Debug.LogWarning("No save file found. Returning default GameData.");
        return new GameData
        {
            lastUnlockedLevel = 1
        };
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted.");
        }
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("deleted");

    }

    public static bool SaveExists()
    {
        return File.Exists(savePath);
    }


}