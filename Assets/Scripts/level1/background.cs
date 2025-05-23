using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public Transform cameraToFollow; // دوربینی که بک‌گراند باید باهاش اسکرول بشه
    public float scrollFactor = 0.1f; // میزان حرکت بک‌گراند نسبت به دوربین

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (cameraToFollow != null)
        {
            float offsetX = cameraToFollow.position.x * scrollFactor;
            mat.mainTextureOffset = new Vector2(offsetX, 0);
        
        }
    }
}