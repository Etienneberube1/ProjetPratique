using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField] private GameObject m_Boss;
    [SerializeField] private Transform m_BossSpawnPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(m_Boss, m_BossSpawnPos.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
