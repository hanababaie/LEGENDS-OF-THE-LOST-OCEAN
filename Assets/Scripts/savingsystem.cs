using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class gamedata
{
    public int lastunlockedlevel;
    public playerdata player1Data;
    public playerdata player2Data;
    public string currentscene;
    public bool isLevel3;
}

[Serializable]
public class playerdata
{
    public int health;
    public int lives;
    public int coins;
    public Vector3 position;
    public bool hasKey;
    public bool hasKey2;
    public bool atShip;
    public bool atFinalDoor;
}


public class savingsystem : MonoBehaviour
{
    public const String savingkey = "GameSaveData";

    public static void savinGame(gamedata data)
    {
        String jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(savingkey, jsonData);
        PlayerPrefs.Save();
    }

    public static gamedata loadingGame()
    {
        if (PlayerPrefs.HasKey(savingkey))
        {
            String jsonData = PlayerPrefs.GetString(savingkey);
            return JsonUtility.FromJson<gamedata>(jsonData);
        }

        return new gamedata()
        {
            lastunlockedlevel = 1 //default
        };
    }

    public static void deleteData()
    {
        PlayerPrefs.DeleteKey(savingkey);
    }
}
