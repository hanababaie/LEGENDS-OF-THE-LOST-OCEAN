using System.Collections;
using UnityEngine;

public class camerapos : MonoBehaviour
{
    public Camera maincamera;
    public Transform cameraforlevel3;
    public GameObject obe;

    public GameObject player1;
    public GameObject player2;
    public bool player1reached = false;
    public bool player2reached = false;
    public bool cameramoved;

    public void playersreached(GameObject player)
    {
       

        if (player == player1)
        {
            player1reached = true;
        }
        if (player == player2)
        {
            player2reached = true;
        }

        if (player1reached && player2reached && !cameramoved)
        {
            cameramoved = true;
            obe.SetActive(false);
            StartCoroutine(movingcamera());
        }
    }

    IEnumerator movingcamera()
    {
        Vector3 initialpos = maincamera.transform.position;
        Vector3 targetpos = new Vector3(cameraforlevel3.position.x, cameraforlevel3.position.y, initialpos.z);

        float timespent = 0f;
        float duration = 2f;

        while (timespent < duration)
        {
            timespent += Time.deltaTime;
            maincamera.transform.position = Vector3.Lerp(initialpos, targetpos, timespent / duration);
            yield return null;
        }

        maincamera.transform.position = targetpos;
    }
}
