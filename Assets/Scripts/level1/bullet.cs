
using UnityEngine;
public class bullet : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public float direction = 1f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(bulletSpeed * direction, 0f);
        float angle = direction > 0 ? -90f : 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle); 
        Invoke("destroy", 2f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            
            EnemyShooting  enemy= collision.gameObject.GetComponent<EnemyShooting>();
            Enemypatrolling enemyp = collision.gameObject.GetComponent<Enemypatrolling>();
            
            
            if (enemy != null && enemyp.isActiveAndEnabled)
            {
                enemy.TakeDamage(2);
                enemyp.TakeDamage(2);
            }
            destroy();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            destroy();
        }
    }

    public void destroy()
    {
        Destroy(gameObject);
    }
}

