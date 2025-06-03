using System;
using UnityEngine;

public class lever : MonoBehaviour
{
    public GameObject cage;
    private SpriteRenderer spriterenderer;
    public Sprite leveron;
    public Sprite leveroff;
    
    public void  Start()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriterenderer.sprite = leveron;
            cage.SetActive(false);

        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        spriterenderer.sprite = leveroff;
        cage.SetActive(true);
    }
}
