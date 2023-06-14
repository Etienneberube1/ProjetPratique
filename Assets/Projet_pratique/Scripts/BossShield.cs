using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour
{
    [SerializeField] private GameObject Crystal_1;
    [SerializeField] private GameObject Shield;
    [SerializeField] private int Crystal_1_hp = 100;
    [SerializeField] GameObject Boss;
    public bool isShieldActive = false;
    void Update()
    {
        if (isShieldActive)
        {
            Crystal_1.gameObject.SetActive(true);
            Shield.gameObject.SetActive(true);
            if (Crystal_1_hp <= 0)
            {
                // Both crystals have been destroyed, deactivate the shield
                DeactivateShield();
            }
        }
        else
        {
            DeactivateShield();
        }
    }

    private void DeactivateShield()
    {
        isShieldActive = false;
        Crystal_1.gameObject.SetActive(false);
        Shield.gameObject.SetActive(false);
    }

    public void DamageCrystal(int damageAmount)
    {
        Crystal_1_hp -= damageAmount;
        if (Crystal_1_hp <= 0)
        {
            Crystal_1.gameObject.SetActive(false);
        }

    }
}
