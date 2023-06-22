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
    private void Start()
    {
        Crystal_1.gameObject.SetActive(false);
        Shield.gameObject.SetActive(false);
    }
    void Update()
    {
        Debug.Log(isShieldActive);
        if (isShieldActive == true)
        {
            Crystal_1.gameObject.SetActive(true);
            Shield.gameObject.SetActive(true);
            if (Crystal_1_hp <= 0)
            {
                // crystals have been destroyed, deactivate the shield
                isShieldActive = false;
                DesactivateShield();
            }
        }
    }
    private void DesactivateShield()
    {
        Crystal_1.gameObject.SetActive(false);
        Shield.gameObject.SetActive(false);

    }

    public void DamageCrystal(int damageAmount)
    {
        Crystal_1_hp -= damageAmount;
    }
}
