using UnityEngine;

public enum ItemType { HealthUpgrade, SpeedUpgrade, ExtraLife }

[System.Serializable]
public class shopitem
{
    public string itemname;
    public ItemType itemtype;
    public int price;
    public float value;
    public Sprite itemIcon;
}