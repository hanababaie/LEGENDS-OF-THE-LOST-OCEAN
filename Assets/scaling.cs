using UnityEngine;

public class UIScaler : MonoBehaviour
{
    public Vector2 referenceResolution = new Vector2(1920, 1080);

    void Update()
    {
        float scaleX = Screen.width / referenceResolution.x;
        float scaleY = Screen.height / referenceResolution.y;
        float scale = Mathf.Min(scaleX, scaleY);

        transform.localScale = new Vector3(scale, scale, 1);
    }
}
