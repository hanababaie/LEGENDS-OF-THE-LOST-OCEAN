using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class lootbag : MonoBehaviour
{

    public GameObject dropitemprefab;
    public List<lootsystem> lootlist = new List<lootsystem>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    lootsystem getdropitem()
    {
        int random = Random.Range(1, 101); // 1 to 100
        List<lootsystem> possible = new List<lootsystem>(); // create a list for possible loots
        foreach (lootsystem item in lootlist)
        {
            if (random <= item.dropchance)
            {
                possible.Add(item); // add to the list
            }
        }

        if (possible.Count > 0)
        {
            lootsystem drop = possible[Random.Range(0, possible.Count)]; // return one random
            return drop;
        }
        return null;
    }


    public void spawndropitem(Vector3 position)
    {
        lootsystem drop = getdropitem();
        Debug.Log("Trying to drop item...");

        if (drop == null)
        {
            Debug.LogWarning("No loot item to drop!");
            return;
        }

        Debug.Log("Dropping item: " + drop.lootname);

        if (dropitemprefab == null)
        {
            Debug.LogError("Drop Item Prefab is not assigned!");
            return;
        }

        GameObject lootGameObject = Instantiate(dropitemprefab, position, Quaternion.identity); // instantite the square
        Debug.Log("Instantiated lootGameObject");

        SpriteRenderer sr = lootGameObject.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = drop.lootsprite; // change the pic and its sprite 
        }
        else
        {
            Debug.LogError("Prefab missing SpriteRenderer!");
        }

        var takingItemsScript = lootGameObject.GetComponent<takingitems>(); // having access to the taking script
        if (takingItemsScript != null)
        {
            takingItemsScript.loot = drop; // pass the loot to it so player can pickit
        }

        Rigidbody2D rb = lootGameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 2f)).normalized * 10f;
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    
    //add for when the loot appears
}

        

}
