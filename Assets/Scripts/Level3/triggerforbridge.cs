
using UnityEngine;

public class BridgeTriggerController : MonoBehaviour
{
    public Camera maincamera;
    public Vector3 newpos = new Vector3(4450,-230,-10);
    public float movespeed = 2f;

    public GameObject wallBlock;

    private bool player1Reached = false;
    private bool player2Reached = false;
    private bool cameramove = false;
    public Vector3 startingpoint = new Vector3(4450,-800,-10);

    public void Start(){
        maincamera.transform.position = startingpoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        
            if (collision.GetComponent<playermovement1>() != null)
            {
                player1Reached = true;
            }
            else if (collision.GetComponent<playermovement2>() != null)
            {
                player2Reached = true;
            }

        
            if (player1Reached && player2Reached)
            {
                if (wallBlock != null)
                {
                    wallBlock.SetActive(false); 
                }

                if (maincamera != null)
                {
                    cameramove = true;
                }
            }
        }
    }

    private void Update()
    {
        if (cameramove)
        {
                maincamera.transform.position = Vector3.Lerp(
                maincamera.transform.position,
                newpos,
                Time.deltaTime * movespeed
            );
            if (Vector3.Distance(maincamera.transform.position, newpos) < 0.1f)
            {
                maincamera.transform.position = newpos;
                cameramove = false;
            }
        }
    }
}
