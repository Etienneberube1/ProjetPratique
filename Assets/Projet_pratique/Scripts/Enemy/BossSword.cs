using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSword : MonoBehaviour
{
    [SerializeField] private int BulletDMG = 5;
    private Boss m_Boss;
    private void Start()
    {
        m_Boss = GetComponentInParent<Boss>();
    }
    private void OnTriggerEnter2D(Collider2D HitInfo)
    {
        if (HitInfo.gameObject.tag == "Player")
        {
            m_Boss.m_Animator.SetTrigger("Attack");
            Player m_Player = HitInfo.GetComponent<Player>();
            m_Player.TakeDamage(BulletDMG);
        }
    }
}
