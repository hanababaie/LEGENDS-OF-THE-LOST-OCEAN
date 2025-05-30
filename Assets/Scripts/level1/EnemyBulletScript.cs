using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force = 5f;
    private float timer;

    public void SetDirection(Vector2 dir)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir.normalized * force;

        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 8f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var pm1 = collision.GetComponent<playermovement1>();
            if (pm1 != null)
            {
                pm1.TakeDamage(2);
                Destroy(gameObject);
                return;
            }

            var pm2 = collision.GetComponent<playermovement2>();
            if (pm2 != null)
            {
                pm2.TakeDamage(2);
                Destroy(gameObject);
            }
        }
    }

}