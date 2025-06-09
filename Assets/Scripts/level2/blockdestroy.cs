using UnityEngine;

public class blockdestroy : MonoBehaviour
{
    public playermovement1 p1;
    public AudioSource audioSource;
    public AudioClip breaking;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && p1.isDashing) // if player1 hit it when dashing
        {
            Destroy();
        }
        if (other.gameObject.CompareTag("bullet")) // if player 2 hit it with bullet
        {
            Destroy();
            Destroy(other.gameObject); // destroy the bullet
        }
    }

    public void Destroy()
    {
        audioSource.PlayOneShot(breaking); // play the music
        Destroy(gameObject);
    }
}
