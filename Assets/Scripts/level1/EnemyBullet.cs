using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, 2f); // نابودی خودکار پس از ۲ ثانیه
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hit = collision.gameObject;

        if (hit.CompareTag("player1"))
        {
            hit.GetComponent<playermovement1>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (hit.CompareTag("player2"))
        {
            hit.GetComponent<playermovement2>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            // اگر به هیچ‌کدام از بازیکن‌ها نخورد، ولی به چیزی دیگر مثل زمین یا دیوار برخورد کرد، نابود شود
            Destroy(gameObject);
        }
    }

}