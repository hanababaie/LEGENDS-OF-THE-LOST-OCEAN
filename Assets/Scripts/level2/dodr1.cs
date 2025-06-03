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
        spriterenderer = GetComponent<SpriteRenderer>();

    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (p1.haskey2)
    {
        Debug.Log("opened");
        opendoor();
    }
}

    public void opendoor()
    {
        audioSource.PlayOneShot(opendoorsound);
        oldtile.SetActive(false);
        gameObject.SetActive(false);

        newtile.SetActive(true);
        portal.SetActive(true);
    }
        
}

