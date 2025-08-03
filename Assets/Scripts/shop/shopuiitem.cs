using TMPro;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;
    public Image itemIcon; 
    public Button buyButton;

    private shopitem item;
    private shop shopManager;
    private int playernum;

    public void Setup(shopitem myitem, shop shop, int playernumb)
    {
        item = myitem;
        shopManager = shop;
        playernum = playernumb;

        itemNameText.text = item.itemname;
        priceText.text = item.price.ToString();
        itemIcon.sprite = item.itemIcon;   

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => shopManager.buying(item, playernum));
    }
    
}