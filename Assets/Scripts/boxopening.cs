using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class boxopening : MonoBehaviour
{
    public List<lootsystem> items;
    Animator animator;

    private bool isopened = false;

    public GameObject boxUI;
  
    private Image Imagebox;
    private TextMeshProUGUI Textbox;

    public GameObject player;

    public void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        
        
        Imagebox =boxUI.transform.Find("Image").GetComponent<Image>();
        Textbox = boxUI.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        boxUI.SetActive(false);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isopened)
        {
            player = other.gameObject;
            isopened = true;
            Debug.Log("opened");
            StartCoroutine(openbox());
            
        }
    }

    IEnumerator openbox()
    {

        animator.SetTrigger("open");
        yield return new WaitForSeconds(1.5f);


        lootsystem item = getrandom();
        
        if (item == null)
        {
          ;
            yield break;
        }
 
        boxUI.SetActive(true);
       
        Imagebox.sprite = item.lootsprite;
        Textbox.text = item.lootname;
        
        GameObject lootObject = new GameObject("TempLoot");
        var take = lootObject.AddComponent<takingitems>();
        take.loot = item;
        take.OnTriggerEnter2D(player.GetComponent<Collider2D>());

        Destroy(lootObject);
 
        
        
        Debug.Log("item: " + item);
        Debug.Log("Sprite: " + item.lootsprite);
        Debug.Log("Name: " + item.lootname);


        yield return new WaitForSeconds(3f);

        boxUI.SetActive(false);
       Destroy(gameObject); 
    }

    lootsystem getrandom()
    {
        int random = UnityEngine.Random.Range(1, 101); // 1 to 100
        List<lootsystem> possible = new List<lootsystem>();
        foreach (lootsystem item in items)
        {
            if (random <= item.dropchance)
            {
                possible.Add(item);
            }
        }

        if (possible.Count == 0)
        {
            return null;
        }
        int index = UnityEngine.Random.Range(0, possible.Count);
        return possible[index];
    }
}

