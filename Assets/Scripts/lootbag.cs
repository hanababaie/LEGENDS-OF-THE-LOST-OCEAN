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
            int random = Random.Range(1 , 101); // 1 to 100
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
            if (drop != null)
            {
                GameObject lootgsmeobject = Instantiate(dropitemprefab, position, Quaternion.identity);

                // اطمینان از وجود SpriteRenderer
                SpriteRenderer sr = lootgsmeobject.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = drop.lootsprite;
                }
                else
                {
                    Debug.LogWarning("SpriteRenderer not found on dropitemprefab!");
                }

                // اطمینان از وجود Rigidbody2D
                Rigidbody2D rb = lootgsmeobject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    float dropforce = 30f;
                    Vector2 dropdirection = new Vector2(Random.Range(1f, 1f), Random.Range(1f, 1f));
                   
                }
                else
                {
                    Debug.LogWarning("Rigidbody2D not found on dropitemprefab!");
                }
            }
        }

}
