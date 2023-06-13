using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearGoblin : MonoBehaviour
{
    [SerializeField] private float m_EnemyHP = 100f;
    [SerializeField] private float m_CantMoveTimer = 0.75f;
    [SerializeField] private float m_Speed = 1f;
    [SerializeField] private Transform m_GroundCheck;

    private float m_StartingHP;
    private bool m_IsEnemyDead = false;
    private bool m_CanMove = true;
    private bool m_IsMovingRight = true;
    private Vector2 m_Direction;

    private SpriteRenderer m_SpriteRender;
    private Animator m_Animator;
    private Rigidbody2D m_RigidBody2D;

    // crystal stuff
    [SerializeField] private GameObject m_CrystalPrefabs;
    private int CrystalSpawned = 0;


    void Start()
    {
        m_StartingHP = m_EnemyHP;
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRender = GetComponent<SpriteRenderer>();
        m_Direction = Vector2.right;
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

        if (m_CanMove == true)
        {
            Movement();
        }
    }

    private void Movement()
    {

        Vector2 movement = m_RigidBody2D.velocity;
        movement = m_Direction * m_Speed;
        RaycastHit2D groundinfo = Physics2D.Raycast(m_GroundCheck.position, Vector2.down, 2f);
        if (!groundinfo.collider)
        {
            if (m_IsMovingRight)
            {
                m_Direction = Vector2.left;
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                m_Direction = Vector2.right;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            m_IsMovingRight = !m_IsMovingRight;
        }
        m_RigidBody2D.velocity = movement;
    }


    public void TakeDamage(int DMG)
    {
        m_EnemyHP -= DMG;
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
