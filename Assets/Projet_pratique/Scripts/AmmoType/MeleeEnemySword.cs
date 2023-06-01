using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemySword : MonoBehaviour
{
    [SerializeField] private int BulletDMG = 5;
    private MeleeEnemy m_Enemy;
    private void Start()
    {
        m_Enemy = GetComponentInParent<MeleeEnemy>();
    }
    private void OnTriggerEnter2D(Collider2D HitInfo)
    {
        if (HitInfo.gameObject.tag == "Player")
        {
            m_Enemy.m_Animator.SetBool("Attack", true);
            Player m_Player = HitInfo.GetComponent<Player>();
            m_Player.TakeDamage(BulletDMG);
        }
    }
    
}
