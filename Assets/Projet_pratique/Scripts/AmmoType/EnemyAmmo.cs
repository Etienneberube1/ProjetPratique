using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAmmo : MonoBehaviour
{
    [SerializeField] private int BulletDMG = 5;
    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D HitInfo)
    {
        if (HitInfo.gameObject.tag == "Player")
        {
            Player m_Player = HitInfo.GetComponent<Player>();
            m_Player.TakeDamage(BulletDMG);
            Destroy(gameObject);
        }
    }
}
