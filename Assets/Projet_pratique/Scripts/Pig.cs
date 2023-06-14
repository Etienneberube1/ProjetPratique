using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pig : MonoBehaviour
{
    [SerializeField] private BossBattle m_BossBattleScript;
    [SerializeField] private float m_EnemyHP = 100f;
    [SerializeField] private float m_CantMoveTimer = 0.75f;
    [SerializeField] private float m_Speed = 1f;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private float m_TimeBetweenAttack = 3f;
    [SerializeField] private int m_Dmg = 20;
    public float m_enemyHP { get { return m_EnemyHP; } }

    private bool isAttacking = false;
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
        StartCoroutine(AttackCoroutine());
    }
    void Update()
    {
        if (m_EnemyHP <= 0)
        {
            m_Animator.SetTrigger("Dead");
            StartCoroutine(SpawnCrystal());
            m_IsEnemyDead = true;
            m_CanMove = false;
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

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            // Check if the enemy is not currently attacking
            if (!isAttacking)
            {
                // Set the attacking flag to true
                isAttacking = true;

                // if the boss is in stage 1 he will do this attack
                if (m_BossBattleScript.m_Stage == BossBattle.Stage.Stage_1)
                {
                    m_Animator.SetTrigger("Attack_1");
                }
                
                // Wait for 3 seconds
                yield return new WaitForSeconds(3f);

                // Set the attacking flag back to false
                isAttacking = false;
            }
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            player.TakeDamage(m_Dmg);
        }
    }
    public void TakeDamage(int DMG)
    {
        m_EnemyHP -= DMG;
        m_Animator.SetTrigger("Hit");
        //m_HealthBar.SetHealth(m_EnemyHP, m_StartingHP);
        m_BossBattleScript.BossBattle_OnDamaged();
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
    public void DestroyPig()
    {
        Debug.Log("sdad");
        Destroy(gameObject, 1f);
    }
    public IEnumerator HitCoroutine()
    {
        m_CanMove = false;
        yield return new WaitForSeconds(m_CantMoveTimer);
        m_CanMove = true;
    }
}

