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
   public float shootRange = 20f;
   public AudioClip shootSound;
   private AudioSource audioSource;
   public AudioClip deathSound; // صدای مرگ

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Animator را مقداردهی می‌کنیم
        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return; 
        shootTimer += Time.deltaTime;
        if (shootTimer >= 5f)
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
            Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            float distance = Vector2.Distance(enemyPos, playerPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = player;
            }
        }
      
        if (nearest != null&&bullet != null)
        {
            // فعال‌سازی انیمیشن حمله
            if (animator != null)
            {
                animator.SetTrigger("attack");
            }

            Vector2 direction = (nearest.transform.position - bulletPos.position).normalized;
            GameObject b = Instantiate(bullet, bulletPos.position, Quaternion.identity);
            b.GetComponent<EnemyBulletScript>().SetDirection(direction);
            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
            
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
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        Destroy(gameObject, 1.5f);
       
    }

   
}
