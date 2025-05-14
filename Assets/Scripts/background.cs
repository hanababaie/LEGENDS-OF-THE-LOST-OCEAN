using UnityEngine;

public class background : MonoBehaviour
{
    [Range(-1f , 1f)] // if reach negative it scroll in the opposite direction
    public float speedforscroll = 1f;

    private float offset;
    private Material mat;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = GetComponent<Renderer>().material; // get the material
    }

    // Update is called once per frame
    void Update()
    {
        offset += (speedforscroll * Time.deltaTime) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
