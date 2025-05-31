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
        List<lootsystem> possible = new List<lootsystem>();
        foreach (lootsystem item in lootlist)
        {
            if (random <= item.dropchance)
            {
                possible.Add(item);
            }
        }

        if (possible.Count > 0)
        {
            lootsystem drop = possible[Random.Range(0, possible.Count)];
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

    GameObject lootGameObject = Instantiate(dropitemprefab, position, Quaternion.identity);
    Debug.Log("Instantiated lootGameObject");

    SpriteRenderer sr = lootGameObject.GetComponent<SpriteRenderer>();
    if (sr != null)
    {
        sr.sprite = drop.lootsprite;
    }
    else
    {
        Debug.LogError("Prefab missing SpriteRenderer!");
    }

    var takingItemsScript = lootGameObject.GetComponent<takingitems>();
    if (takingItemsScript != null)
    {
        takingItemsScript.loot = drop;
    }

    Rigidbody2D rb = lootGameObject.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 2f)).normalized * 10f;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}

        

}
