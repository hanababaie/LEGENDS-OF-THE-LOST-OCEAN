
using UnityEngine;
public class bullet : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public float direction = 1f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(bulletSpeed * direction, 0f); //set the movement of the bullet
        float angle = direction > 0 ? -90f : 90f;
        //when facing right -90 and when not 90 degree
        transform.rotation = Quaternion.Euler(0f, 0f, angle);  // rotating the bullet
        Invoke("destroy", 2f); // distroy the bullet after 2 sec
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        { 
            EnemyShooting enemy = collision.gameObject.GetComponent<EnemyShooting>();
            //it hurts the enemy with shooting script
            if (enemy != null && enemy.isActiveAndEnabled)
            {
                enemy.TakeDamage(2);
            }

            Enemypatrolling enemyp = collision.gameObject.GetComponent<Enemypatrolling>();
            // it hurnt the enemy with patrolling script
            if (enemyp != null && enemyp.isActiveAndEnabled)
            {
                enemyp.TakeDamage(2);
            }

            destroy(); // destroy the bullet
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            destroy(); // destroy the bullet
        }
    }


    public void destroy()
    {
        Destroy(gameObject);
    }
}

