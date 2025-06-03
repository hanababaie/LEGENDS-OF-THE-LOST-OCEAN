using UnityEngine;

public class portal : MonoBehaviour
{
    public Transform destinationPortal;
    private bool istele = false;
    public float cooldown = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !istele)
        {
            StartCoroutine(Teleport(other));
        }
    }

    private System.Collections.IEnumerator Teleport(Collider2D player)
    {
        istele = true;

        portal destPortal = destinationPortal.GetComponent<portal>();
        if (destPortal != null)
        {
            destPortal.istele = true;
        }


        player.transform.position = destinationPortal.position;

        yield return new WaitForSeconds(cooldown);
        istele = false;

        if (destPortal != null)
        {
            yield return new WaitForSeconds(cooldown);
            destPortal.istele = false;
        }
    }
}
