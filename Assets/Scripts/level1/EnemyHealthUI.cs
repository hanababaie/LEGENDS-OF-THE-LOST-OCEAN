using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Image healthImage;
    
    public Sprite health100;
    public Sprite health75;
    public Sprite health50;
    public Sprite health25;
    public Sprite health0;

    public void UpdateHealthBar(float healthPercent)
    {
        if (healthPercent >= 0.99f)
            healthImage.sprite = health100;
        else if (healthPercent >= 0.74f)
            healthImage.sprite = health75;
        else if (healthPercent >= 0.49f)
            healthImage.sprite = health50;
        else if (healthPercent >= 0.24f)
            healthImage.sprite = health25;
        else
            healthImage.sprite = health0;
    }
}