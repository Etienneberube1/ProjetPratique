using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private int m_ImpactDmg = 20;
    [SerializeField] private int m_OverTimeDmg = 5;
    [SerializeField] private int m_TimeBetweenTickDmg = 2;
    private Animator m_Animator;
    private Rigidbody2D m_Body2D;

    private void Start()
    {
        Destroy(gameObject, 3f);
        m_Animator = GetComponent<Animator>();
        m_Body2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D HitInfo)
    {
        m_Body2D.velocity = Vector2.zero;
        m_Animator.SetTrigger("Impact");
        if (HitInfo.gameObject.tag == "Enemy")
        {
            Enemy Enemy = HitInfo.GetComponent<Enemy>();
            Enemy.TakeOverTimeDamage(m_ImpactDmg, m_OverTimeDmg, m_TimeBetweenTickDmg);
            
        }
        else if (HitInfo.gameObject.tag == "MeleeEnemy")
        {
            MeleeEnemy Enemy = HitInfo.GetComponent<MeleeEnemy>();
            Enemy.TakeOverTimeDamage(m_ImpactDmg, m_OverTimeDmg, m_TimeBetweenTickDmg);
            StartCoroutine(Enemy.HitCoroutine());
        }
        else if (HitInfo.gameObject.tag == "Boss")
        {
            Boss m_Boss = HitInfo.GetComponent<Boss>();
            m_Boss.TakeOverTimeDamage(m_ImpactDmg, m_OverTimeDmg, m_TimeBetweenTickDmg);
            StartCoroutine(m_Boss.HitCoroutine());
            //Destroy(gameObject, 0.42f);
        }

    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
