using UnityEngine;

public class dodr1 : MonoBehaviour
{
    public playermovement1 p1;
    public playermovement2 p2;

    public AudioClip opendoorsound;
    public AudioSource audioSource;

    public GameObject portal;
    private SpriteRenderer spriterenderer;

    public GameObject oldtile;
    public GameObject newtile;


    public void Start()
    {
        spriterenderer = GetComponent<SpriteRenderer>(); // take the sprite renderer

    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (p1.haskey2) // if player 1 has the key we can open the door 
    {
        Debug.Log("opened");
        opendoor();
    }
}

    public void opendoor()
    {
        audioSource.PlayOneShot(opendoorsound);

        oldtile.SetActive(false); // turn of the old set tile 
        gameObject.SetActive(false);

        newtile.SetActive(true); // active the new tiles
        portal.SetActive(true); // active the portal
    }
        
}

