using UnityEngine;

public class moveableblock : MonoBehaviour
{
    public Vector3 direction = Vector3.right;
    public float speed = 2f;
    public float distance = 5f;

    private Vector3 startPosition;
    private bool movingforward = true;
    private float traveleddistance = 0f;

    void Start()
    {
        startPosition = transform.position; // take the start pos
    }

    void Update()
    {
        float moveStep = speed * Time.deltaTime; //caculate how much to move this frame

        if (movingforward) // when we move forward
        {
            transform.Translate(direction * moveStep); // move forforward
            traveleddistance += moveStep; // how much we moves

            if (traveleddistance >= distance) // if we move enough
                movingforward = false;
        }
        else
        {
            transform.Translate(-direction * moveStep); // move in the opposite direction
            traveleddistance -= moveStep; // we updae it

            if (traveleddistance <= 0) // when move enough
                movingforward = true; // move forward again
        }
    }
}
