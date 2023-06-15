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
    [SerializeField] private int Dmg = 15;
    private Animator m_Animator;
    private bool m_IsEnemyDead = false;
    private AIDestinationSetter m_Target;
    private AIPath m_AiPathScript;

    private GameObject m_Player;
    private float DistanceBetweenPlayer;
    // crystal stuff
    [SerializeField] private GameObject m_CrystalPrefabs;
    private int CrystalSpawned = 0;

    private void Start()
    {
        // setting hp 
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        m_AiPathScript = GetComponent<AIPath>();
        m_Animator = GetComponent<Animator>();
        m_Target = GetComponent<AIDestinationSetter>();

        // will change later with playermanager
        m_Player = GameObject.FindGameObjectWithTag("Player");
        if (m_Player != null)
        {
            m_Target.target = m_Player.transform;
        }
    }
    private void Update()
    {
        Attack();
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
        DistanceBetweenPlayer = Vector3.Distance(transform.position, m_Player.transform.position);
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
