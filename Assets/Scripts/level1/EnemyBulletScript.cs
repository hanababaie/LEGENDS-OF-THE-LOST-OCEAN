using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force = 5f;
    private float timer;
    public GameObject explosionEffect;

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
            if (explosionEffect != null)
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
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
                pm1.TakeDamage(1);
                if (explosionEffect != null)
                {
                    GameObject expl = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    // Animator خودش کارشو می‌کنه — destruction هم داخل خود اون prefab انجام میشه
                }

                Destroy(gameObject);
                return;
            }

            var pm2 = collision.GetComponent<playermovement2>();
            if (pm2 != null)
            {
                pm2.TakeDamage(1);
                if (explosionEffect != null)
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

}