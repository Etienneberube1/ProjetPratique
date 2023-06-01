using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float m_TimeBetweenSpawn = 2f;
    [SerializeField] private float m_TimeTrapIsOut = 3f;
    [SerializeField] private GameObject TrapPrefab;
    void Start()
    {
        StartCoroutine(TrapCoroutine());
        InvokeRepeating("TrapSpawn", 0f, m_TimeBetweenSpawn);
    }

    private void TrapSpawn()
    {
        StartCoroutine(TrapCoroutine());
    }

    // Spawn Trap each X amount of time
    IEnumerator TrapCoroutine()
    {
        yield return new WaitForSeconds(m_TimeBetweenSpawn);
        SpawnTrap();
    }
    private void SpawnTrap()
    {
        GameObject newTrap = Instantiate(TrapPrefab, gameObject.transform);
        Destroy(newTrap, 1f);
    }
}
