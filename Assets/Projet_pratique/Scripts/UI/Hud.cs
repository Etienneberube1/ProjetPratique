using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    [SerializeField] private Text Ammo_Text;
    [SerializeField] private Text HP_Text;
    [SerializeField] private Text m_Crystal_Text;
    private Animator m_Animator;
    private void Start()
    {
        UIManager.Instance.OnAmmoChange += OnAmmoChange;
        UIManager.Instance.OnHPChange += OnHPChange;
        UIManager.Instance.OnCrystalChange += OnCrystalChange;
        m_Animator = GetComponent<Animator>();
    }
    private void OnCrystalChange(int CrystalAmount)
    {
        m_Crystal_Text.text = ($"{(CrystalAmount)}");
    }
    private void OnAmmoChange(int CurrentAmmo, int MaxAmmo)
    {
        Ammo_Text.text = CurrentAmmo + "/" + MaxAmmo;
    }

    private void OnHPChange(int CurrentHP)
    {
        HP_Text.text = ($"{(CurrentHP)}");
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("GameOver");
    }
    private void OnDestroy()
    {
        UIManager.Instance.OnHPChange -= OnHPChange;
        UIManager.Instance.OnAmmoChange -= OnAmmoChange;
        UIManager.Instance.OnCrystalChange -= OnCrystalChange;
    }
}
