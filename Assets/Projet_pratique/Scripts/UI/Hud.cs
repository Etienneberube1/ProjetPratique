using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    [SerializeField] private TMP_Text Ammo_Text;
    [SerializeField] private TMP_Text HP_Text;
    [SerializeField] private Image m_FadeImage;
    [SerializeField] private TMP_Text m_Crystal_Text;
    private Animator m_Animator;
    private void Start()
    {
        UIManager.Instance.OnAmmoChange += OnAmmoChange;
        UIManager.Instance.OnHPChange += OnHPChange;
        UIManager.Instance.OnPlayerDeath += FadeToBlack;
        UIManager.Instance.OnCrystalChange += OnCrystalChange;
        m_Animator = GetComponent<Animator>();
    }
    private void OnCrystalChange(int CrystalAmount)
    {
        m_Crystal_Text.text = ($": {(CrystalAmount)}");
    }
    private void OnAmmoChange(int CurrentAmmo, int MaxAmmo)
    {
        Ammo_Text.text = CurrentAmmo + "/" + MaxAmmo;
    }

    private void OnHPChange(int CurrentHP)
    {
        HP_Text.text = ($"HP: {(CurrentHP)}");
    }
    private void FadeToBlack(bool IsPlayerDead)
    {
        if (IsPlayerDead == true)
        {
            m_Animator.SetTrigger("FadeToBlack");
        }
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("GameOver");
    }
    private void OnDestroy()
    {
        UIManager.Instance.OnHPChange -= OnHPChange;
        UIManager.Instance.OnAmmoChange -= OnAmmoChange;
        UIManager.Instance.OnPlayerDeath -= FadeToBlack;
    }
}
