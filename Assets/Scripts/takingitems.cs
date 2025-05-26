using System;
using UnityEngine;

public class takingitems : MonoBehaviour
{
    public GameObject itemdroped;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"));
        {
            Destroy(gameObject);
        } 
    }
}
