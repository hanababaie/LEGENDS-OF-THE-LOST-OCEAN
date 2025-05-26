using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private Animator anim;

    private EnemyHealthUI healthUI;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>(); 
        // گرفتن کامپوننت UI سلامت دشمن
        healthUI = GetComponentInChildren<EnemyHealthUI>();
        healthUI?.UpdateHealthBar(1f); // مقدار اولیه: 100٪

        EnemyManager.instance?.RegisterEnemy(this);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // بروزرسانی UI سلامت
        if (healthUI != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            healthUI.UpdateHealthBar(healthPercent);
        }
        anim.SetTrigger("hit");
        // بررسی مرگ
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<lootbag>().spawndropitem(transform.position);
        EnemyManager.instance?.UnregisterEnemy(this);
        anim.SetTrigger("die");
        Destroy(gameObject);
        
    }

    public void SetHealth(int health)
    {
        currentHealth = health;

        if (healthUI != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            healthUI.UpdateHealthBar(healthPercent);
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}