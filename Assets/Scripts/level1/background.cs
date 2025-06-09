using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public Transform cameraToFollow; //camera to follow
    public float scrollFactor = 0.1f; // how much it moves

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
            mat.mainTextureOffset = new Vector2(offsetX, 0); // give the scrolling effect
        
        }
    }
}