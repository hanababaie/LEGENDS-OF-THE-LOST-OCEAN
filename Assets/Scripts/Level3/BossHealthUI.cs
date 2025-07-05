using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Slider healthSlider;

    public void UpdateHealthBar(float healthPercent)
    {
        healthSlider.value = healthPercent;
    }
}