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
        slider.value = Maxheslth; // fill the slider 
        
    }


    public void Setmaxhealth(int max)
    {
        this.Maxheslth = max;
        slider.value = max;
        slider.maxValue = max;
        bar.color = gradient.Evaluate(1f); // set the color to the max health

    }
    public void Sethealth(int health)
    {
        slider.value = health; // set the health
        bar.color = gradient.Evaluate(slider.normalizedValue); // set the color based on the bars sitaution
    }
}
