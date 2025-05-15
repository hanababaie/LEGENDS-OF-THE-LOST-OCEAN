using UnityEngine;

public class background : MonoBehaviour
{

    private float offset;
    private Material mat;
    public float speedforsroll = 0.5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = GetComponent<Renderer>().material; // get the material
    }

    // Update is called once per frame
    void Update()
    {
        offset += (speedforsroll * Time.deltaTime) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
