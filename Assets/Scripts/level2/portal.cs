using UnityEngine;

public class portal : MonoBehaviour
{
    public Transform destinationPortal;
    private bool istele = false;
    public float cooldown = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !istele) // when we hit it and we are not teleporting
        {
            StartCoroutine(Teleport(other));
        }
    }

    private System.Collections.IEnumerator Teleport(Collider2D player)
    {
        istele = true; // we are teleporting

        portal destPortal = destinationPortal.GetComponent<portal>(); // take the componnet of it
        if (destPortal != null)
        {
            destPortal.istele = true; // now portal2 is busy
        }


        player.transform.position = destinationPortal.position; // transform the player

        yield return new WaitForSeconds(cooldown); // wait to do it again
        istele = false;

        if (destPortal != null)
        {
            yield return new WaitForSeconds(cooldown);
            destPortal.istele = false;
        }
    }
}
