using UnityEngine;

public class TopDownEnemyShooting : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject bullet;
    public Transform bulletPos;
    private float shootTimer;

    public float detectionRadius = 20f;
    public float shootInterval = 3f;

    public AudioClip shootSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    private Animator animator;
    private bool isDead = false;

    public EnemyHealthUI healthUI;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return;

        GameObject target = GetNearestPlayerInRadius();

        if (animator != null)
        {
            animator.SetBool("isMoving", target != null);
        }

        if (target != null)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                ShootAtPlayer(target);
            }
        }
    }

    GameObject GetNearestPlayerInRadius()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= detectionRadius && distance < minDistance)
            {
                minDistance = distance;
                nearest = player;
            }
        }

        return nearest;
    }

    void ShootAtPlayer(GameObject player)
    {
        if (player == null || bullet == null) return;

        if (animator != null)
        {
            animator.SetTrigger("attack");
        }

        Vector2 direction = (player.transform.position - bulletPos.position).normalized;
        GameObject b = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        b.GetComponent<EnemyBulletScript>().SetDirection(direction);

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (animator != null)
        {
            animator.SetTrigger("hit");
        }

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
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("die");
        }

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        Destroy(gameObject, 1.5f);
        GetComponent<lootbag>()?.spawndropitem(transform.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
