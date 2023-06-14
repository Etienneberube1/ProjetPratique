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

        //ingore the collsion between the crystal and the bullet
        Physics2D.IgnoreLayerCollision(6, 7);
    }
    private void OnTriggerEnter2D(Collider2D HitInfo)
    {
        m_Body2D.velocity = Vector2.zero;
        m_Animator.SetTrigger("Impact");
        if (HitInfo.gameObject.tag == "Enemy")
        {
            Roller enemy = HitInfo.GetComponent<Roller>();
            enemy.TakeDamage(m_BulletDMG);
        }
        else if (HitInfo.gameObject.tag == "MeleeEnemy")
        {
            SpearGoblin Enemy = HitInfo.GetComponent<SpearGoblin>();
            Enemy.TakeDamage(m_BulletDMG);
        }
        else if (HitInfo.gameObject.tag == "Pig")
        {
            Pig pig = HitInfo.GetComponent<Pig>();
            pig.TakeDamage(m_BulletDMG);
        }
        else if (HitInfo.gameObject.tag == "Crystal")
        {
            BossShield shieldScript = HitInfo.GetComponentInParent<BossShield>();
            shieldScript.DamageCrystal(m_BulletDMG);
        }
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
