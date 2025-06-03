using UnityEngine;

public class blockdestroy : MonoBehaviour
{
    public playermovement1 p1;
    public AudioSource audioSource;
    public AudioClip breaking;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && p1.isDashing)
        {
            Destroy();
        }
        if (other.gameObject.CompareTag("bullet"))
        {
            Destroy();
            Destroy(other.gameObject);
        }
    }

    public void Destroy()
    {
        audioSource.PlayOneShot(breaking);
        Destroy(gameObject);
    }
}
