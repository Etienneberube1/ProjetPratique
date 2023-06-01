using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float m_EnemyHP = 100f;
    private float m_StartingHP;
    [SerializeField] private float m_EnemySpeed = 5f;
    [SerializeField] private float m_TimeBetweenShoot = 2f;
    [SerializeField] private float m_EnemyBulletSpeed;
    [SerializeField] private Transform m_FirePoint;
    [SerializeField] private GameObject m_BulletPrefab;
    [SerializeField] private Player m_Player;
    [SerializeField] private GameObject[] m_ItemdropList;
    //[SerializeField] private SpawnManager m_SpawnManagerScript;
    private Animator m_Animator;
    private Transform m_WeaponTransform;
    private bool m_OverTimeCoroutineIsRunning = false;
    private bool m_SlowCoroutineIsRunning = false;
    private bool m_IsEnemyDead = false;
    private SpriteRenderer m_SpriteRender;
    [HideInInspector] public Rigidbody2D m_Rigidbody2D;
    [SerializeField] private HealthBarManager m_HealthBar; 
    


    private void Awake()
    {
        m_WeaponTransform = transform.Find("Weapon");
    }

    private void Start()
    {
        m_StartingHP = m_EnemyHP;
        m_HealthBar.SetHealth(m_EnemyHP, m_StartingHP);
        InvokeRepeating("Shoot", 0f, m_TimeBetweenShoot );
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
            //m_SpawnManagerScript.EnemyDied();
        }
    }
    //Shooting ============================================================
    private void Shoot()
    {
        StartCoroutine(ShootCoroutine());
    }
    
    IEnumerator ShootCoroutine()
    {
        m_TimeBetweenShoot = Random.Range(m_TimeBetweenShoot - 1, m_TimeBetweenShoot + 1);
        m_Animator.SetTrigger("Attack");
        yield return new WaitForSeconds(m_TimeBetweenShoot);
        EnemyShooting();
    }
    private void EnemyShooting()
    {
        GameObject Bullet = Instantiate(m_BulletPrefab, m_FirePoint.position, m_FirePoint.rotation);
        Rigidbody2D m_Rigidbody2D = Bullet.GetComponent<Rigidbody2D>();
        m_Rigidbody2D.AddForce(m_FirePoint.right * m_EnemyBulletSpeed, ForceMode2D.Impulse);
    }
    //====================================================================


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
            m_Animator.SetTrigger("IsDead");
            Destroy(gameObject, 1);
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
    }
    //public IEnumerator HitCoroutine()
    //{
    //    m_CanMove = false;
    //    yield return new WaitForSeconds(m_NumberOfTimePlayerCantMove);
    //    m_CanMove = true;
    //}
    //=====================================================================
}
