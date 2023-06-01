 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{
    [SerializeField] public Transform FirePoint;
    [SerializeField] public GameObject BulletPrefab;
    [SerializeField] public GameObject WeaponPrefab;
    [SerializeField] public float m_BulletForce = 8f;
    [SerializeField] public float m_FireRate = 1f;
    [SerializeField] public int MaxAmmo = 50;
    private protected int CurrentAmmo;
    private protected bool CanShoot = true;
    private protected Animator m_Animator;
    public void Start()
    {
        CurrentAmmo = MaxAmmo;
        m_Animator = GetComponent<Animator>();
    }
    public void Update()
    {
        UIManager.Instance.AmmoChange(CurrentAmmo, MaxAmmo);
        if (Input.GetButton("Fire1") && CurrentAmmo != 0 && CanShoot == true)
        {
            StartCoroutine(FireRateCoroutine());
        }
    }

    IEnumerator FireRateCoroutine()
    {
        Shoot();
        CanShoot = false;
        yield return new WaitForSeconds(m_FireRate);
        CanShoot = true;
    }
    public virtual void Shoot()
    {
        m_Animator.SetTrigger("Shoot");
        GameObject Bullet =  Instantiate(BulletPrefab, FirePoint.transform.position, FirePoint.rotation);
        Rigidbody2D m_Rigidbody2D = Bullet.GetComponent<Rigidbody2D>();
        m_Rigidbody2D.AddForce(FirePoint.right * m_BulletForce, ForceMode2D.Impulse);
        UIBulletManager();
    }

    public void UIBulletManager()
    {
        CurrentAmmo--;
    }

    private void OnTriggerEnter2D(Collider2D HitInfo)
    {
        if (HitInfo.gameObject.tag == "Player")
        {
            Player player = HitInfo.GetComponent<Player>();
            player.OnChangeGun(WeaponPrefab);
            Destroy(gameObject, 0.2f);
            GetComponent<Gun>().enabled = true;
        }
    }
}
