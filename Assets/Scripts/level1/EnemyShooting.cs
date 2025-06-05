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
    
    public AudioClip shootSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    // این دو برای تعیین محدوده دید
    public Transform leftViewLimit;
    public Transform rightViewLimit;
    


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return;

        GameObject target = GetNearestPlayerInSight();
        if (animator != null)
        {
            animator.SetBool("isMoving", target != null); ///درحالت idle  میمونه
        }

        if (target != null)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= 5f)
            {
                shootTimer = 0f;
                ShootAtPlayer(target);
            }
        }
    }

    // ✅ بررسی نزدیک‌ترین بازیکن در محدوده دید
    GameObject GetNearestPlayerInSight()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float px = player.transform.position.x;
            float left = leftViewLimit.position.x;
            float right = rightViewLimit.position.x;

            // اطمینان از درست بودن جهت چپ و راست
            if (left > right)
            {
                float temp = left;
                left = right;
                right = temp;
            }

            if (px >= left && px <= right)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = player;
                }
            }
        }

        return nearest;
    }

    // ✅ شلیک به بازیکن خاص
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
        Destroy(gameObject,1.5f);
    }
  

}
