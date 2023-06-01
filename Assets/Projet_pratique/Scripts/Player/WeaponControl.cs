using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponControl : MonoBehaviour
{

    private Transform m_WeaponTransform;
    private SpriteRenderer m_SpriteRenderer;
    private Player m_PlayerScripts;

    private void Awake()
    {
        m_WeaponTransform = transform.Find("WeaponRotation");
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_PlayerScripts = GetComponent<Player>();
    }
    void Update()
    {
        AimingHandler();
    }

    private void AimingHandler()
    {
        Vector3 MousePos = GetMouseWorldPosition();

        Vector3 AimDirection = (MousePos - transform.position).normalized;
        float angle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg;
        m_WeaponTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 WeaponlocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            if (m_PlayerScripts.m_WallCheckFlip == false) {
                m_PlayerScripts.Flip();
            }
            m_SpriteRenderer.flipX = true;
            WeaponlocalScale.y = -1f;
        }
        else
        {
            if (m_PlayerScripts.m_WallCheckFlip == true) {
                m_PlayerScripts.Flip();
            }

            m_SpriteRenderer.flipX = false;
            WeaponlocalScale.y = +1f;
        }
        m_WeaponTransform.localScale = WeaponlocalScale;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static Vector3 GetDirToMouse(Vector3 fromPosition)
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        return (mouseWorldPosition - fromPosition).normalized;
    }
}
