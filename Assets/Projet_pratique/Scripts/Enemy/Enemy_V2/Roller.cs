using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    private Animator m_Animator;
    private int m_HP = 100;
    private bool m_IsEnemyDead = false;
    
    // crystal stuff
    [SerializeField] private GameObject m_CrystalPrefabs;
    private int CrystalSpawned = 0;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (m_HP <= 0)
        {
            m_Animator.SetTrigger("Dead");
            m_IsEnemyDead = true;
            StartCoroutine(SpawnCrystal());
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<Rigidbody2D>());
        }
    }
    public void TakeDamage(int DMG)
    {
        m_HP -= DMG;
        m_Animator.SetTrigger("Hit");
        //m_HealthBar.SetHealth(m_EnemyHP, m_StartingHP);
    }
    private IEnumerator SpawnCrystal()
    {
        int RandomCrystalAmount = Random.Range(1, 4);
        yield return new WaitForSeconds(0.2f);
        if (CrystalSpawned <= RandomCrystalAmount && m_IsEnemyDead != false)
        {
            GameObject Crystal = Instantiate(m_CrystalPrefabs, transform.position, m_CrystalPrefabs.transform.rotation);
            CrystalSpawned++;
        }
    }
    public void DestroyRoller()
    {
        Destroy(gameObject);
    }

}
