using UnityEngine;

public class followcamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10); // camera being in the behind

    void LateUpdate() // it is called after all other functions so we first can move 
    {
        if (target != null)
        {
            Vector3 newPosition = target.position + offset;
            newPosition.z = -10f;
            transform.position = newPosition;
        }
    }
}

