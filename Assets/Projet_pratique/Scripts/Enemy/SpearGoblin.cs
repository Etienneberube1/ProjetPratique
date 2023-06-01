using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearGoblin : MonoBehaviour
{
    [SerializeField] private float m_EnemyHP = 100f;
    [SerializeField] private float m_CantMoveTimer = 0.75f;
    private Rigidbody2D m_RigidBody2D;
    [HideInInspector] public Animator m_Animator;
    private float m_StartingHP;
    private bool m_IsEnemyDead = false;
    private SpriteRenderer m_SpriteRender;
    private bool m_CanMove = true;

    [SerializeField] private GameObject m_CrystalPrefabs;

    void Start()
    {
        m_StartingHP = m_EnemyHP;
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRender = GetComponent<SpriteRenderer>();

    }
    void Update()
    {
        if (m_EnemyHP <= 0)
        {
            StartCoroutine(SpawnCrystal());
            m_Animator.SetTrigger("Dead");
            m_IsEnemyDead = true;
            m_CanMove = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<Rigidbody2D>());
        }
    }
    public void TakeDamage(int DMG)
    {
        m_EnemyHP -= DMG;
        m_Animator.SetTrigger("Hit");
        //m_HealthBar.SetHealth(m_EnemyHP, m_StartingHP);
    }
    int CrystalSpawned = 0;

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
    public void DestroyEnemy()
    {
        Destroy(gameObject, 1f);
    }

    public IEnumerator HitCoroutine()
    {
        m_CanMove = false;
        yield return new WaitForSeconds(m_CantMoveTimer);
        m_CanMove = true;
    }
}
