using System;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;


public class healthbar : MonoBehaviour
{
    public Slider slider;
    public float Maxheslth = 10;

    public Gradient gradient;

    public Image bar;

    public void Start()
    {
        slider.value = Maxheslth;
        
    }


    public void Setmaxhealth(int max)
    {
        this.Maxheslth = max;
        slider.value = max;
        slider.maxValue = max;
        bar.color = gradient.Evaluate(1f);

    }
    public void Sethealth(int health)
    {
        slider.value = health;
        bar.color = gradient.Evaluate(slider.normalizedValue);
    }
}
