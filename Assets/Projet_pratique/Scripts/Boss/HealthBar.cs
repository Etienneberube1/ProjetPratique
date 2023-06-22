using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fill;
    public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;

    }
    public void SetHealth(int health)
    {
        _slider.value = health;

    }
}
