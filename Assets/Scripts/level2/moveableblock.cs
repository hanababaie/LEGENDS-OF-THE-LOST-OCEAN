using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    public Vector3 direction = Vector3.right;
    public float speed = 2f;
    public float distance = 5f;

    private Vector3 startPosition;
    private bool movingForward = true;
    private float traveledDistance = 0f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float moveStep = speed * Time.deltaTime;

        if (movingForward)
        {
            transform.Translate(direction * moveStep);
            traveledDistance += moveStep;

            if (traveledDistance >= distance)
                movingForward = false;
        }
        else
        {
            transform.Translate(-direction * moveStep);
            traveledDistance -= moveStep;

            if (traveledDistance <= 0)
                movingForward = true;
        }
    }
}
