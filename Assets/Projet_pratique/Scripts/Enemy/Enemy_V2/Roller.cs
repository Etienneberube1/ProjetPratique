using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class Roller : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int CurrentHealth;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float DetectionRange = 1.5f;
    [SerializeField] private float MaxDetectionRange = 30f;
    [SerializeField] private int Dmg = 15;
    private Animator m_Animator;
    private bool m_IsEnemyDead = false;

    private AIDestinationSetter m_Target;
    private AIPath m_AiPathScript;
    private int PlayerHP;
    private Player _player;
    private float DistanceBetweenPlayer = 20;

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

    private void Start()
    {
        GetPlayerRef();
        // setting hp 
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        m_AiPathScript = GetComponent<AIPath>();
        m_Animator = GetComponent<Animator>();
        m_Target = GetComponent<AIDestinationSetter>();

        m_Target.target = _player.transform;
    }
    private void Update()
    {
        DistanceBetweenPlayer = Vector3.Distance(_player.transform.position, transform.position);
        Attack();
        if (PlayerHP <= 0)
        {
            Destroy(gameObject);
        }

        if (DistanceBetweenPlayer >= MaxDetectionRange)
        {
            m_AiPathScript.canMove = false;
        }
        else { m_AiPathScript.canMove = true; }



        if (CurrentHealth <= 0)
        {
            m_Animator.SetTrigger("Dead");
            m_IsEnemyDead = true;
            StartCoroutine(SpawnCrystal());
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<Rigidbody2D>());
        }
    }
    private void Attack()
    {
        if (DistanceBetweenPlayer <= DetectionRange)
        {
            m_AiPathScript.maxSpeed = 6f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DestroyRoller();
            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(Dmg);
        }
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
    public void DestroyRoller()
    {
        AudioManager.Instance.PlaySFX(AudioManager.EAudio.EnemyDying);
        Destroy(gameObject, 0.2f);
    }
    private void OnPlayerSet(Player player)
    {
        _player = player;
        PlayerHP = player.m_PlayerHP;
    }
    private void OnDestroy()
    {
        PlayerManager.Instance.PlayerData -= OnPlayerSet;
    }
}
