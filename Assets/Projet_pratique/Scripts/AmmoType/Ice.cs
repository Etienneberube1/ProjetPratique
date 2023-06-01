using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    [SerializeField] private int m_ImpactDmg = 20;
    [SerializeField] private int m_SlowAmount = 5;
    [SerializeField] private int m_TimeSlowed = 2;
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
            Enemy enemy = HitInfo.GetComponent<Enemy>();
            enemy.TakeIceDamage(m_ImpactDmg, m_SlowAmount, m_TimeSlowed);
        }
        else if (HitInfo.gameObject.tag == "MeleeEnemy")
        {
            MeleeEnemy Enemy = HitInfo.GetComponent<MeleeEnemy>();
            Enemy.TakeIceDamage(m_ImpactDmg, m_SlowAmount, m_TimeSlowed);
            StartCoroutine(Enemy.HitCoroutine());
        }
        else if (HitInfo.gameObject.tag == "Boss")
        {
            Boss m_Boss = HitInfo.GetComponent<Boss>();
            m_Boss.TakeIceDamage(m_ImpactDmg, m_SlowAmount, m_TimeSlowed);
            StartCoroutine(m_Boss.HitCoroutine());
            //Destroy(gameObject, 0.42f);
        }

    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
