using UnityEngine;

public class Enemypatrolling : MonoBehaviour
{
    public int damage = 1;
    public float speed = 2f;
    public float patrolRange = 5f;

    private Vector2 startPoint;
    private bool movingRight = true;

    void Start()
    {
        startPoint = transform.position;
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= startPoint.x + patrolRange)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= startPoint.x - patrolRange)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playermovement1 pm1 = collision.gameObject.GetComponent<playermovement1>();
            if (pm1 != null)
            {
                pm1.TakeDamage(damage);
                return;
            }

            playermovement2 pm2 = collision.gameObject.GetComponent<playermovement2>();
            if (pm2 != null)
            {
                pm2.TakeDamage(damage);
            }
        }
    }
}