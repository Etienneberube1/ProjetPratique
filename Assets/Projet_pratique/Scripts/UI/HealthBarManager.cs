using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private Slider m_Slider;
    [SerializeField] private Color m_Low;
    [SerializeField] private Color m_High;
    public void SetHealth(float Health, float MaxHealth)
    {
        if (Health < MaxHealth)
        {
            m_Slider.gameObject.SetActive(true);
        }
        else
        {
            m_Slider.gameObject.SetActive(false);
        }
        m_Slider.value = Health;
        m_Slider.maxValue = MaxHealth;

        m_Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(m_Low, m_High, m_Slider.normalizedValue);
    }
}
