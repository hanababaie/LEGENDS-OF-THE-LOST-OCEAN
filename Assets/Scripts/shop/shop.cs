
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.Rendering;

public class shop : MonoBehaviour
{
    public List<shopitem> items;
    public Transform player1ItemsParent;
    public Transform player2ItemsParent;
    public GameObject shopItemPrefab;


    public GameObject player1panel;
    public GameObject player2panel;

    public TMPro.TextMeshProUGUI player1CoinsText;
    public TMPro.TextMeshProUGUI player2CoinsText;

    private PlayerData playerData1;
    private PlayerData playerData2;

    public int currentPanel = 1;

    public playermovement1 p1;
    public playermovement2 p2;

    void Start()
    {
        loadPlayerData();
        showpanel(1);
        showUIpanel();
        updatecoins();

    }

    public void loadPlayerData()
    {
        playerData1 = new PlayerData()
        {
            coins = PlayerPrefs.GetInt("Player1_Coins", p1.coins),
            maxhealth = PlayerPrefs.GetInt("Player1_MaxHealth", p1.maxHealth),
            maxspeed = PlayerPrefs.GetFloat("Player1_MoveSpeed", p1.movespeed),
            extraLife = PlayerPrefs.GetInt("Player1_ExtraLives", 0)
        };

        playerData2 = new PlayerData()
        {
            coins = PlayerPrefs.GetInt("Player2_Coins", p2.coins),
            maxhealth = PlayerPrefs.GetInt("Player2_MaxHealth", p2.maxHealth),
            maxspeed = PlayerPrefs.GetFloat("Player2_MoveSpeed", p2.movespeed),
            extraLife = PlayerPrefs.GetInt("Player2_ExtraLives", 0)
        };
    }

    void SavePlayerData()
    {
        PlayerPrefs.SetInt("Player1_Coins", playerData1.coins);
        PlayerPrefs.SetInt("Player1_MaxHealth", playerData1.maxhealth);
        PlayerPrefs.SetFloat("Player1_MoveSpeed", playerData1.maxspeed);
        PlayerPrefs.SetInt("Player1_ExtraLives", playerData1.extraLife);

        PlayerPrefs.SetInt("Player2_Coins", playerData2.coins);
        PlayerPrefs.SetInt("Player2_MaxHealth", playerData2.maxhealth);
        PlayerPrefs.SetFloat("Player2_MoveSpeed", playerData2.maxspeed);
        PlayerPrefs.SetInt("Player2_ExtraLives", playerData2.extraLife);

        PlayerPrefs.Save();
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

        if (playerData.coins >= item.price)
        {
            playerData.coins -= item.price;

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
                p1.maxHealth = playerData1.maxhealth;
                p1.movespeed = playerData1.maxspeed;
                p1.currentHealth = p1.maxHealth;
                p1.bar.Setmaxhealth(p1.maxHealth);
                p1.bar.Sethealth(p1.currentHealth);
                p1.currentlives += item.itemtype == ItemType.ExtraLife ? (int)item.value : 0;
                p1.updatevives();
            }
            else
            {
                p2.maxHealth = playerData2.maxhealth;
                p2.movespeed = playerData2.maxspeed;
                p2.currentHealth = p2.maxHealth;
                p2.bar.Setmaxhealth(p2.maxHealth);
                p2.bar.Sethealth(p2.currentHealth);
                p2.currentlives += item.itemtype == ItemType.ExtraLife ? (int)item.value : 0;
                p2.updatelives();
            }

            SavePlayerData();
            updatecoins();
            Debug.Log($"Player {playernum} bought {item.itemname}");
        }
        else
        {
            Debug.Log("not enough coins");
        }
    }

    void updatecoins()
    {
        player1CoinsText.text = $"{playerData1.coins}";
        player2CoinsText.text = $"{playerData2.coins}";
    }

    public void NextPanel()
    {
        // اگر روی Player1 هستی، برو Player2
        if (currentPanel == 1)
            showpanel(2);
        else
            showpanel(1);
    }

    public void PreviousPanel()
    {
        // اگر روی Player2 هستی، برگرد Player1
        if (currentPanel == 2)
            showpanel(1);
        else
            showpanel(2);
    }




}
