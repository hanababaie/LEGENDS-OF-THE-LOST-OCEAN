using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject bullet;
    public Transform bulletPos;
    private float shootTimer;

    public EnemyHealthUI healthUI;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Animator را مقداردهی می‌کنیم
        UpdateHealthUI();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > 2f)
        {
            shootTimer = 0f;
            ShootAtNearestPlayer();
        }
    }

    void ShootAtNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;

        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = player;
            }
        }

        if (nearest != null)
        {
            // فعال‌سازی انیمیشن حمله
            if (animator != null)
            {
                animator.SetTrigger("attack");
            }

            Vector2 direction = (Vector2)(nearest.transform.position - bulletPos.position);
            GameObject b = Instantiate(bullet, bulletPos.position, Quaternion.identity);
            b.GetComponent<EnemyBulletScript>().SetDirection(direction);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthUI != null)
        {
            healthUI.UpdateHealthBar(currentHealth / maxHealth);
        }
    }

    void Die()
    {
        GetComponent<lootbag>().spawndropitem(transform.position);
        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("die");
        }

        Destroy(gameObject, 1.5f);
    }

   
}
