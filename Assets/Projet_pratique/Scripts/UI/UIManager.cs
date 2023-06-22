using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    public event Action<int , int> OnAmmoChange;
    public event Action<int> OnHPChange;
    public event Action<bool> OnPlayerDeath;
    public event Action<int> OnCrystalChange;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void LifeChange(int CurrentHP)
    {
        OnHPChange?.Invoke(CurrentHP);
    }
    public void AmmoChange(int CurrentAmmo, int MaxAmmo)
    {
        OnAmmoChange?.Invoke(CurrentAmmo, MaxAmmo);
    }

    public void CrystalChange(int CrystalAmount)
    {
        OnCrystalChange?.Invoke(CrystalAmount);
    }
}
