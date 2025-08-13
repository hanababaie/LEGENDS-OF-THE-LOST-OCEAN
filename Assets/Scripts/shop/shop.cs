using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror.BouncyCastle.Asn1.Misc;
using UnityEngine.SceneManagement;

public class shop : MonoBehaviour
{
    public List<shopitem> items;
    public Transform player1ItemsParent;
    public Transform player2ItemsParent;
    public GameObject shopItemPrefab;

    public GameObject player1panel;
    public GameObject player2panel;

    public TextMeshProUGUI player1CoinsText;
    public TextMeshProUGUI player2CoinsText;

    private PlayerData playerData1;
    private PlayerData playerData2;

    public int currentPanel = 1;

    public playermovement1 p1;
    public playermovement2 p2;

    void Start()
    {
        LoadPlayerData();
        showpanel(1);
        showUIpanel();     
        updatecoins();        
    }

    public void Update()
    {
        updatecoins();
    }

    public void LoadPlayerData()
    {
        if (SaveManager.SaveExists())
        {
            GameData data = SaveManager.LoadGame();
            playerData1 = data.player1Data;
            playerData2 = data.player2Data;
        }
        else
        {
            playerData1 = new PlayerData()
            {
                coins = p1.totalcoins,
                maxhealth = p1.maxHealth,
                maxspeed = p1.movespeed,
                extraLife = 0
            };

            playerData2 = new PlayerData()
            {
                coins = p2.totalcoins,
                maxhealth = p2.maxHealth,
                maxspeed = p2.movespeed,
                extraLife = 0
            };
        }
    }

    public void SavePlayerData()
    {

        GameData data = new GameData
        {
            lastUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1),
            currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            isLevel3 = p1.isLevel3 || p2.isLevel3,
            player1Data = playerData1,
            player2Data = playerData2
        };

        SaveManager.SaveGame(data);
    }

    public void showpanel(int playernum)
    {
        currentPanel = playernum;
        player1panel.SetActive(playernum == 1);
        player2panel.SetActive(playernum == 2);
    }

    public void showUIpanel()
    {
        foreach (Transform child in player1ItemsParent) Destroy(child.gameObject);
        foreach (Transform child in player2ItemsParent) Destroy(child.gameObject);

        foreach (var item in items)
        {
            var obj1 = Instantiate(shopItemPrefab, player1ItemsParent);
            var ui1 = obj1.GetComponent<ShopItemUI>();
            ui1.Setup(item, this, 1);

            var obj2 = Instantiate(shopItemPrefab, player2ItemsParent);
            var ui2 = obj2.GetComponent<ShopItemUI>();
            ui2.Setup(item, this, 2);
        }
    }

    public void buying(shopitem item, int playernum)
    {
        PlayerData playerData = playernum == 1 ? playerData1 : playerData2;

        if (playerData.totalcoins >= item.price)
        {
            playerData.totalcoins -= item.price;
            

            switch (item.itemtype)
            {
                case ItemType.HealthUpgrade:
                    playerData.maxhealth += (int)item.value;
                    break;
                case ItemType.SpeedUpgrade:
                    playerData.maxspeed += item.value;
                    break;
                case ItemType.ExtraLife:
                    playerData.extraLife += (int)item.value;
                    break;
            }

            if (playernum == 1)
            {
                p1.totalcoins = playerData.totalcoins; // coins player update
                p1.maxHealth = playerData.maxhealth;
                p1.movespeed = playerData.maxspeed;
                p1.currentHealth = p1.maxHealth;
                p1.bar.Setmaxhealth(p1.maxHealth);
                p1.bar.Sethealth(p1.currentHealth);
                p1.currentlives += item.itemtype == ItemType.ExtraLife ? (int)item.value : 0;
                p1.updatevives();
            }
            else
            {
                p2.totalcoins = playerData.totalcoins; // coins player update
                p2.maxHealth = playerData.maxhealth;
                p2.movespeed = playerData.maxspeed;
                p2.currentHealth = p2.maxHealth;
                p2.bar.Setmaxhealth(p2.maxHealth);
                p2.bar.Sethealth(p2.currentHealth);
                p2.currentlives += item.itemtype == ItemType.ExtraLife ? (int)item.value : 0;
                p2.updatelives();
            }

            SavePlayerData(); // save after applying changes to real players
            updatecoins();
        }
        else
        {
            Debug.Log("not enough coins");
        }
    }

    void updatecoins()
    {
        player1CoinsText.text = playerData1.totalcoins.ToString();
        player2CoinsText.text = playerData2.totalcoins.ToString();
    }

    public void NextPanel()
    {
        showpanel(currentPanel == 1 ? 2 : 1);
    }

    public void PreviousPanel()
    {
        showpanel(currentPanel == 2 ? 1 : 2);
    }

    public void backtomain()
    {
        SavePlayerData();
        SceneManager.LoadScene("mianmenu");
    }
}
