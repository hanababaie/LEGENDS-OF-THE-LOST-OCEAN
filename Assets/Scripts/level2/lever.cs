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
        if (other.CompareTag("Player")) // if player collide with it 
        {
            spriterenderer.sprite = leveron; // change the sprite to the on mode
            cage.SetActive(false); // disapear the cage 

        }
    }
    public void OnTriggerExit2D(Collider2D other) // when we don't hit the lever
    {
        spriterenderer.sprite = leveroff; // change the sprite
        cage.SetActive(true);
    }
}
