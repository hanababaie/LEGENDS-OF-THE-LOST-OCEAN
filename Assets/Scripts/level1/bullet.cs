using UnityEditor.Timeline.Actions;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("destroy" , 1f);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(bulletSpeed, 0);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground") || collision.gameObject.CompareTag("enemy"))
        {
            destroy();
        }
    }

    public void destroy()
    {
        Destroy(gameObject);
    }
}
