using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapStat : MonoBehaviour
{
    [SerializeField] private int TrapDmg = 5;
    private void OnTriggerEnter2D(Collider2D HitInfo)
    {
        if (HitInfo.gameObject.tag == "Player")
        {
            Player m_Player = HitInfo.GetComponent<Player>();
            m_Player.TakeDamage(TrapDmg);
        }
    }
}
