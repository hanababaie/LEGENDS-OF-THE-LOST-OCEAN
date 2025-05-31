using UnityEngine;

public class Enemypatrolling : MonoBehaviour
{
    public int damage = 5;
    public float speed = 2f;
    public float patrolRange = 5f;

    public int maxHealth = 5;
    private int currentHealth;

    public EnemyHealthUI healthUI; // ریفرنس به UI سلامت
    private Animator animator;

    private Vector2 startPoint;
    private bool movingRight = true;
    private bool isDead = false;

    void Start()
    {
        startPoint = transform.position;
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        if (healthUI != null)
            healthUI.UpdateHealthBar(1f);
    }

    void Update()
    {
        if (!isDead)
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
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            playermovement1 pm1 = collision.gameObject.GetComponent<playermovement1>();
            if (pm1 != null)
            {
                pm1.TakeDamage(damage);
              
                
            }

            playermovement2 pm2 = collision.gameObject.GetComponent<playermovement2>();
            if (pm2 != null)
            {
                pm2.TakeDamage(damage);
             
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthUI != null)
            healthUI.UpdateHealthBar((float)currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("spawn");
        GetComponent<lootbag>().spawndropitem(transform.position);
        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("die");
        }

        // بعد از چند ثانیه نابود شود
        Destroy(gameObject, 1.5f);
    }
}
