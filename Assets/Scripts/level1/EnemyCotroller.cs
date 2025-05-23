using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private EnemyHealthUI healthUI;

    void Start()
    {
        currentHealth = maxHealth;

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

        // بررسی مرگ
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        EnemyManager.instance?.UnregisterEnemy(this);
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