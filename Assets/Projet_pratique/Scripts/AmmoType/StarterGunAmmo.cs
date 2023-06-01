using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterGunAmmo : MonoBehaviour
{
    [SerializeField] private int m_BulletDMG = 20;
    private Animator m_Animator;
    private Rigidbody2D m_Body2D;
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Body2D = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3f);
    }
    private void OnTriggerEnter2D(Collider2D HitInfo)
    {
        m_Body2D.velocity = Vector2.zero;
        m_Animator.SetTrigger("Impact");
        if (HitInfo.gameObject.tag == "Enemy")
        {
            Enemy m_Enemy = HitInfo.GetComponent<Enemy>();
            m_Enemy.TakeDamage(m_BulletDMG);
        }
        else if (HitInfo.gameObject.tag == "MeleeEnemy")
        {
            MeleeEnemy m_Enemy = HitInfo.GetComponent<MeleeEnemy>();
            m_Enemy.TakeDamage(m_BulletDMG);
            StartCoroutine(m_Enemy.HitCoroutine());

        }
        else if (HitInfo.gameObject.tag == "Boss")
        {
            Boss m_Boss = HitInfo.GetComponent<Boss>();
            m_Boss.TakeDamage(m_BulletDMG);
            StartCoroutine(m_Boss.HitCoroutine());
        }
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
