using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        EnemyManager.instance?.RegisterEnemy(this);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
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
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}