using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpearGoblin : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int CurrentHealth;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int _Dmg = 20;

    [SerializeField] private float m_CantMoveTimer = 0.75f;
    [SerializeField] private float m_Speed = 1f;
    [SerializeField] private Transform m_GroundCheck;

    [SerializeField] private LayerMask m_PlayerLayer;
    [SerializeField] private Transform m_PlayerCheck;
    [SerializeField] private float m_PlayerCheckRadius;


    private bool m_IsEnemyDead = false;
    private bool m_CanMove = true;
    private bool m_IsMovingRight = true;
    private Vector2 m_Direction;

    private GameObject m_Player;
    private float DistanceBetweenPlayer;

    private SpriteRenderer m_SpriteRender;
    private Animator m_Animator;
    private Rigidbody2D m_RigidBody2D;
    private int playerHP;
    private Player _player;
    // crystal stuff
    [SerializeField] private GameObject m_CrystalPrefabs;
    private int CrystalSpawned = 0;
    private void GetPlayerRef()
    {
        _player = PlayerManager.Instance.GetPlayer();
        if (_player != null)
        {
            OnPlayerSet(_player);
        }
        PlayerManager.Instance.PlayerData -= OnPlayerSet;
        PlayerManager.Instance.PlayerData += OnPlayerSet;
    }
    private void OnPlayerSet(Player player)
    {
        _player = player;
        playerHP = player.m_PlayerHP;
    }
    void Start()
    {
        GetPlayerRef();
        // setting hp stuff
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRender = GetComponent<SpriteRenderer>();
        m_Direction = Vector2.right;
    }
    void Update()
    {
        if (playerHP <= 0)
        {
            Destroy(gameObject);
        }
        Attack();
        if (CurrentHealth <= 0)
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
            m_Animator.SetBool("Run", true);
        }
        else { m_Animator.SetBool("Run", false); }
    }

    private void GetPlayerHp(int playerHp)
    {
        playerHP = playerHp;
    }

    private void Attack()
    {
        if (PlayerCheck())
        {
            m_Animator.SetTrigger("attack");
        }
        DebugDrawCircle(m_PlayerCheck.position, m_PlayerCheckRadius, Color.blue);
    }
    public void PlayAttackSound()
    {
        AudioManager.Instance.PlaySFX(AudioManager.EAudio.AttackGoblin); 
    }
    private bool PlayerCheck()
    {
        return Physics2D.OverlapCircle(m_PlayerCheck.position, m_PlayerCheckRadius, m_PlayerLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.TakeDamage(_Dmg);
        }
    }
    private void DebugDrawCircle(Vector2 center, float radius, Color color)
    {
        const float deltaTheta = 0.1f;
        float theta = 0f;
        Vector2 previousPoint = center + new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta));

        for (theta = deltaTheta; theta < Mathf.PI * 2f; theta += deltaTheta)
        {
            Vector2 point = center + new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta));
            Debug.DrawLine(previousPoint, point, color);
            previousPoint = point;
        }

        // Draw the last segment to close the circle
        Debug.DrawLine(previousPoint, center + new Vector2(radius, 0f), color);
    }
    private void Movement()
    {

        Vector2 movement = m_RigidBody2D.velocity;
        movement = m_Direction * m_Speed;
        RaycastHit2D groundInfo = Physics2D.Raycast(m_GroundCheck.position, Vector2.down, 2f);
        if (!groundInfo.collider)
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
        CurrentHealth -= DMG;
        m_Animator.SetTrigger("Hit");
        healthBar.SetHealth(CurrentHealth);
        AudioManager.Instance.PlaySFX(AudioManager.EAudio.HitSound);
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
    public IEnumerator HitCoroutine()
    {
        m_CanMove = false;
        yield return new WaitForSeconds(m_CantMoveTimer);
        m_CanMove = true;
    }
    public void DestroyEnemy()
    {
        AudioManager.Instance.PlaySFX(AudioManager.EAudio.EnemyDying);
        Destroy(gameObject, 0.2f);
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.PlayerData -= OnPlayerSet;
    }
}
