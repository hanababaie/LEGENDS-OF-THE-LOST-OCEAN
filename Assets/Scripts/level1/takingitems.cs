using UnityEngine;

public class takingitems : MonoBehaviour
{
    public lootsystem loot;
    public GameObject itemdrop;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var p1 = other.GetComponent<playermovement1>();
            var p2 = other.GetComponent<playermovement2>();
            if (p1 != null) // if player has script 1
            {
                switch (loot.lootname)
                {
                    case "health":
                        other.GetComponent<playermovement1>().addhealth(1);
                        break;
                    case "coins":
                        other.GetComponent<playermovement1>().addcoin(50);
                        break;
                    case "speed":
                        other.GetComponent<playermovement1>().addspeed(20);
                        break;
                    case "Gold" :
                        other.GetComponent<playermovement1>().addcoin(100);
                        break;
                    case "New Life":
                        other.GetComponent<playermovement1>().addlives(1);
                        break;
                    case "Speed bosster" :
                        other.GetComponent<playermovement1>().addspeed(35);
                        break;
                    case "Healing":
                        other.GetComponent<playermovement1>().addhealth(1);
                        break;
                    case "Power booster":
                        other.GetComponent<playermovement1>().addpower(10);
                        break;
                    
                }
            }

            if (p2 != null) // if player has script 2
            {
                switch (loot.lootname)
                {
                    case "health":
                        other.GetComponent<playermovement2>().addhealth(1);
                        break;
                    case "coins":
                        other.GetComponent<playermovement2>().addcoin(50);
                        break;
                    case "speed":
                        other.GetComponent<playermovement2>().addspeed(20);
                        break;
                    case "Gold" :
                        other.GetComponent<playermovement2>().addcoin(100);
                        break;
                    case "New Life":
                        other.GetComponent<playermovement2>().addlives(1);
                        break;
                    case "Speed bosster" :
                        other.GetComponent<playermovement2>().addspeed(35);
                        break;
                    case "Healing":
                        other.GetComponent<playermovement2>().addhealth(1);
                        break;
                    case "Power booster":
                        other.GetComponent<playermovement2>().addpower(10);
                        break;
                }
            }

            
            Destroy(gameObject, 1f);
        }
    }
}
