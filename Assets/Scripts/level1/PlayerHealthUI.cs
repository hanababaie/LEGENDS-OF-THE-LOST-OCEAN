using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{

    public int maxHealth = 10;
    public int currentHealth;

    public Image healthImage;

    public Sprite health100;
    public Sprite health75;
    public Sprite health50;
    public Sprite health25;
    public Sprite health0;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        float percent = (float)currentHealth / maxHealth;

        if (percent > 0.75f)
            healthImage.sprite = health100;
        else if (percent > 0.5f)
            healthImage.sprite = health75;
        else if (percent > 0.25f)
            healthImage.sprite = health50;
        else if (percent > 0f)
            healthImage.sprite = health25;
        else
            healthImage.sprite = health0;
    }

    void Die()
    {
        // کارهای مرگ: انیمیشن، غیرفعال کردن کنترل و غیره
        Debug.Log($"{gameObject.name} is dead");
    }
}
