using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float m_NumberOfTimePlayerCantMove = 0.75f;
    [SerializeField] private float m_EnemyHP = 100f;
    [SerializeField] private HealthBarManager m_HealthBar;
    [SerializeField] private float m_EnemySpeed = 2f;
    [SerializeField] private float m_TimeBetweenAttack = 2f;
    [SerializeField] private Player m_Player;
    [SerializeField] private GameObject[] m_ItemdropList;
    private Transform m_WeaponTransform;
    private bool m_OverTimeCoroutineIsRunning = false;
    private bool m_SlowCoroutineIsRunning = false;
    private Rigidbody2D m_RigidBody2D;
    private Vector2 m_Movement;
    [HideInInspector] public Animator m_Animator;
    private float m_StartingHP;
    private bool m_IsEnemyDead = false;
    private SpriteRenderer m_SpriteRender;
    private bool m_CanMove = true;

    private void Awake()
    {
        m_WeaponTransform = transform.Find("Weapon");
    }

    private void Start()
    {
        m_StartingHP = m_EnemyHP;
        m_HealthBar.SetHealth(m_EnemyHP, m_StartingHP);
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        //m_SpawnManagerScript = GetComponent<SpawnManager>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRender = GetComponent<SpriteRenderer>();
        m_Player = FindObjectOfType<Player>().GetComponent<Player>();
    }
    void Update()
    {
        AimingHandler();
        if (m_EnemyHP <= 0) 
        { 
            Die();
        }
    }
    //Drop system=========================================================
    private void DropSysteme()
    {
        int Randomizer = Random.Range(0, m_ItemdropList.Length);
        GameObject DropItems = Instantiate(m_ItemdropList[Randomizer], transform.position, transform.rotation);
    }
    //====================================================================


    //Taking Dmg + dying==================================================
    public void TakeDamage(int DMG)
    {
        m_EnemyHP -= DMG;
        m_Animator.SetTrigger("GotHit");
        m_HealthBar.SetHealth(m_EnemyHP, m_StartingHP);
    }
    //==================ice dmg + Slow====================================
    public void TakeIceDamage(int HitDmg, int SlowAmount, int SlowTime)
    {
        //frist impact 
        m_EnemyHP -= HitDmg;
        m_Animator.SetTrigger("GotHit");
        m_HealthBar.SetHealth(m_EnemyHP, m_StartingHP);
        //changing speed with slow amount
        if (m_SlowCoroutineIsRunning == false)
        {
            StartCoroutine(SlowingEnemyCoroutine(SlowAmount, SlowTime));
        }
    }
    IEnumerator SlowingEnemyCoroutine(int SlowAmount, int SlowTime)
    {
        m_SlowCoroutineIsRunning = true;
        float SaveEnemySpeed = m_EnemySpeed;
        m_EnemySpeed = m_EnemySpeed / SlowAmount;
        yield return new WaitForSeconds(SlowTime);
        m_EnemySpeed = SaveEnemySpeed;
        m_SlowCoroutineIsRunning = false;
    }
    //====================================================================

    public void TakeOverTimeDamage(int HitDmg, int OverTimeDmg, int TimeBetweenTick)
    {
        //frist impact dmg
        m_EnemyHP -= HitDmg; 
        m_Animator.SetTrigger("GotHit");
        m_HealthBar.SetHealth(m_EnemyHP, m_StartingHP);
        //over time dmg
        if (m_OverTimeCoroutineIsRunning == false)
        {
            StartCoroutine(OverTimeDmgCoroutine(OverTimeDmg, TimeBetweenTick));
        }

    }
    IEnumerator OverTimeDmgCoroutine(int OverTimeDmg, int TimeBetweenTick)
    {
        //this.m_SpriteRender.color = GetComponent<Renderer>().material.color = new Color (0, 255, 0, 50);
        m_OverTimeCoroutineIsRunning = true;
        while (m_EnemyHP > 0)
        {
            yield return new WaitForSeconds(TimeBetweenTick);
            m_EnemyHP -= OverTimeDmg;
            m_HealthBar.SetHealth(m_EnemyHP, m_StartingHP);
        }
        m_OverTimeCoroutineIsRunning = false;
    }
    private void Die()
    {
        if (m_IsEnemyDead == false)
        {
            m_IsEnemyDead = true;
            m_CanMove = false;
            m_Animator.SetTrigger("IsDead");
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(gameObject, 0.6f);
            DropSysteme();
        }
    }
    //====================================================================


    //Aiming Controller======================================================
    private void AimingHandler()
    {
        Vector3 AimDirection = (m_Player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg;
        m_WeaponTransform.eulerAngles = new Vector3(0, 0, angle);
        Vector3 WeaponlocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            WeaponlocalScale.y = -1f;
            m_SpriteRender.flipX = true;
        }
        else
        {
            WeaponlocalScale.y = +1f;
            m_SpriteRender.flipX = false;
        }
        m_WeaponTransform.localScale = WeaponlocalScale;
        AimDirection.Normalize();
        m_Movement = AimDirection;
    }
    private void FixedUpdate()
    {
        MoveTowardPlayer(m_Movement);
    }
    private void MoveTowardPlayer(Vector2 Direction)
    {
        if (m_CanMove == true && m_IsEnemyDead == false)
        {
            m_RigidBody2D.MovePosition((Vector2)transform.position + (Direction * m_EnemySpeed * Time.deltaTime));
        }
        
        if (Direction != Vector2.zero)
        {
            m_Animator.SetBool("IsRunning", true);
        }
        else
        {
            m_Animator.SetBool("IsRunning", false);
        }
    }
    public IEnumerator HitCoroutine()
    {
        m_CanMove = false;
        yield return new WaitForSeconds(m_NumberOfTimePlayerCantMove);
        m_CanMove = true;
    }
    //=====================================================================
}
