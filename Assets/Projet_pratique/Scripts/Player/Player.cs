using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float m_Speed = 5f;
    [SerializeField] public int m_PlayerHP = 100;
    [SerializeField] private Transform m_Hand;
    [SerializeField] private Transform m_WeaponSpawnOffset;
    [HideInInspector]public int m_StartingPlayerHP;
    [SerializeField] private int m_CrystalCollected = 0;
    [SerializeField] private int m_KnockBackForce = 2;

    private float m_HorizontalMouvement;
    private Rigidbody2D m_Body2D;
    private Animator m_Animator;
    private Vector2 Direction = Vector2.zero;
    private GameObject m_WeaponPrefab;
    private SpriteRenderer m_SpriteRenderer;
    private bool m_playerIsMoving = false;

    //variable for the jump
    [SerializeField] private float m_JumpForce = 3f;
    [SerializeField] private float m_JumpTime;
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private Transform m_GroundCheck;
    private bool m_Grounded = false;
    private float m_JumptimeCounter;
    private bool m_IsJumping = false;

    // variable for the wall slide and wall jump
    [SerializeField] private LayerMask m_WallLayer;
    [SerializeField] private Transform m_WallCheck;
    [SerializeField]private float m_WallSlideSpeed = 2f;
    private bool m_IsWallSliding;
    private bool m_IsWallJumping;
    private float m_WallJumpingDirection;
    [SerializeField] private float m_WallJumpingTime = 0.2f;
    private float m_WallJumpingCounter;
    [SerializeField] private float m_WallJumpingDuration = 0.4f;
    [SerializeField]private Vector2 m_WallJumpingPower = new Vector2(8f, 16f);
    [SerializeField] private Transform m_WallCheckFlipPoint;
    public bool m_WallCheckFlip;


    void Start()
    {
        UIManager.Instance.LifeChange(m_PlayerHP);
        m_StartingPlayerHP = m_PlayerHP;

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Body2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    }
    void Update()
    {
        WallSlide();
        WallJump();
        if (!m_IsWallJumping) {
            PlayerControl();
        }

        if (m_PlayerHP <= 0) { 
            m_PlayerHP = 0;
            Die();
        }
    }
    //Taking DMG + Die + Heal=====================================================
    public void Heal(int Heal)
    {
        //will change player hp when he collect heal
        if (m_PlayerHP < m_StartingPlayerHP){
            m_PlayerHP += Heal;
            UIManager.Instance.LifeChange(m_PlayerHP);
        }
        //make sure the player hp dosent go above is cap
        if (m_PlayerHP >= m_StartingPlayerHP) {
            m_PlayerHP = m_StartingPlayerHP;
            UIManager.Instance.LifeChange(m_PlayerHP);
        }
    }
    public void CrystalAdded(int CrystalAmount)
    {
        m_CrystalCollected += CrystalAmount;
        UIManager.Instance.CrystalChange(CrystalAmount);
    }
    public void TakeDamage(int DMG)
    {
        m_PlayerHP -= DMG;
        UIManager.Instance.LifeChange(m_PlayerHP);
        m_Animator.SetTrigger("GotHit");
        if (m_PlayerHP <= 0) {
            Die();
            m_Animator.SetTrigger("Dead");
        }
    }
    private void Die()
    {
        UIManager.Instance.IsPlayerDead(true);
        Destroy(m_Body2D);
    }
    //==============================================================================



    //Player Control================================================================
    private void PlayerControl()
    {
        // Movement for the player

        m_HorizontalMouvement = Input.GetAxis("Horizontal");
        Vector2 velocity = m_Body2D.velocity;
        if (m_HorizontalMouvement > 0 || m_HorizontalMouvement < 0)
        {
            velocity.x = m_HorizontalMouvement * m_Speed;
            m_Body2D.velocity = velocity;
        }

        if (m_HorizontalMouvement > 0 || m_HorizontalMouvement < 0) {
            m_playerIsMoving = true;
        }
        else { m_playerIsMoving = false; }

        if (Input.GetButtonUp("Horizontal") && m_playerIsMoving == true) {
            m_playerIsMoving = false;
            m_Animator.SetTrigger("StopRun");
        }
        // Player jump
        m_Grounded = Physics2D.Raycast(m_GroundCheck.position, Vector2.down, 0.1f, m_GroundLayer);
        if (Input.GetKeyDown(KeyCode.Space) && m_Grounded) {
            m_playerIsMoving = false;
            m_Grounded = false;
            m_IsJumping = true;
            m_Body2D.AddForce(Vector2.up * m_JumpForce, ForceMode2D.Impulse);
            m_Animator.SetTrigger("jump");
        }
        //if (IsJumping == true && Input.GetKey(KeyCode.Space)) {
        //    if (m_JumptimeCounter > 0) {
        //        m_Body2D.AddForce(Vector2.up * m_JumpForce, ForceMode2D.Impulse);
        //        m_JumptimeCounter -= Time.deltaTime;
        //    }
        //    else { IsJumping = false; }
        //}
        if (Input.GetKeyUp(KeyCode.Space)) { m_IsJumping = false; }

        // Changing the sprite to face the right way
        if (m_HorizontalMouvement > 0  && m_WallCheckFlip) { 
            m_SpriteRenderer.flipX = false;
            Flip();
        }
        else if (m_HorizontalMouvement < 0 && !m_WallCheckFlip) { 
            m_SpriteRenderer.flipX = true;
            Flip();
        }

        // Changing the anim to run
        if (m_playerIsMoving == true && !IsWalled() && !m_IsJumping) { m_Animator.SetBool("running", true); }
        else { m_Animator.SetBool("running", false); }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            if (m_Grounded == true && m_playerIsMoving == false) {
                m_Animator.SetTrigger("HitGround");
            }
            
        }
    }
    public void Flip()
    {

        Vector3 localScale = m_WallCheckFlipPoint.transform.localScale;
        localScale.x *= -1;
        m_WallCheckFlipPoint.transform.localScale = localScale;
        m_WallCheckFlip = !m_WallCheckFlip;
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(m_WallCheck.position, 0.1f, m_WallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !m_Grounded && m_HorizontalMouvement != 0) {
            m_IsWallSliding = true;
            m_Body2D.velocity = new Vector2(m_Body2D.velocity.x, Mathf.Clamp(m_Body2D.velocity.y, -m_WallSlideSpeed, float.MaxValue));
        }
        else { m_IsWallSliding = false; }

        // changing the animator if the player is on a wall or not
        if (IsWalled()) { m_Animator.SetBool("isOnWall", true);}
        else { m_Animator.SetBool("isOnWall", false); }

    }

    private void WallJump()
    {
        if (m_IsWallSliding) {
            m_IsWallSliding = false;
            m_WallJumpingDirection = -m_WallCheckFlipPoint.transform.localScale.x;
            m_WallJumpingCounter = m_WallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else {
            m_WallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && m_WallJumpingCounter > 0f) {
            m_IsWallJumping = true;
            m_Body2D.velocity = new Vector2(m_WallJumpingDirection * m_WallJumpingPower.x, m_WallJumpingPower.y);
            m_WallJumpingCounter = 0f;

            if (m_WallJumpingDirection != 1) {
                m_SpriteRenderer.flipX = true;
            }
            else {
                m_SpriteRenderer.flipX = false;
            }
        }

        Invoke(nameof(StopWallJumping), m_WallJumpingDuration);
    }

    private void StopWallJumping()
    {
        m_IsWallJumping = false;
    }


    //==========================================================================
    
    //Pickup New Gun============================================================
    public void OnChangeGun(GameObject WeaponType)
    {
        StartCoroutine(ChangeEquipement(WeaponType));
    }
    IEnumerator ChangeEquipement(GameObject WeaponType)
    {
        while (m_Hand.transform.childCount > 0){
            Destroy(m_Hand.transform.GetChild(0).gameObject);
            yield return null;
        }
        GameObject NewWeapon = Instantiate(WeaponType, m_WeaponSpawnOffset.transform.position, m_Hand.rotation);
        NewWeapon.transform.parent = m_Hand;
        m_WeaponPrefab = NewWeapon;
    }

    public void KnockBack()
    {
        if (m_SpriteRenderer.flipX == true)
        {
            m_Body2D.AddForce(Vector2.right * m_KnockBackForce, ForceMode2D.Impulse);
        }
        else if(m_SpriteRenderer.flipX == false)
        {
            m_Body2D.AddForce(Vector2.left * m_KnockBackForce, ForceMode2D.Impulse);
        }
    }
    //===========================================================================
}