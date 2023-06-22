using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
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
    private Player m_PlayerScript;
    private GameObject m_Player;
    private float DistanceBetweenPlayer = 20;

    // crystal stuff
    [SerializeField] private GameObject m_CrystalPrefabs;
    private int CrystalSpawned = 0;
    private void Start()
    {
        // setting hp 
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        // will change later with playermanager
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_AiPathScript = GetComponent<AIPath>();
        m_Animator = GetComponent<Animator>();
        m_Target = GetComponent<AIDestinationSetter>();

        if (m_Player != null)
        {
            m_Target.target = m_Player.transform;
            m_PlayerScript = m_Player.GetComponent<Player>();
        }
    }
    private void Update()
    {
        DistanceBetweenPlayer = Vector3.Distance(transform.position, m_Player.transform.position);
        Attack();
        if (m_PlayerScript.PlayerHP <= 0 && m_Player != null)
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
        if (m_Player != null)
        {
            if (DistanceBetweenPlayer <= DetectionRange)
            {
                m_AiPathScript.maxSpeed = 6f;
            }
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

}
