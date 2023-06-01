using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]private GameObject[] m_EnemyPrefab;
    [SerializeField]private Transform[] m_EnemySpawnPoint;
    private int m_RoomIndex = 0;
    private int m_NumberOfEnemyToSpawn = 0;
    private bool m_CanChangeRoom = false;
    private int m_TotalNumberOfRoom = 0;
    
    private void Start()
    {
        m_NumberOfEnemyToSpawn = m_EnemySpawnPoint.Length - 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_NumberOfEnemyToSpawn = m_EnemySpawnPoint.Length - 1;
            EnemySpawner();
            Destroy(this);
            //m_RoomIndex++;
            //if (m_RoomIndex < m_TotalNumberOfRoom)
            //{
            //    transform.position = m_RoomAnchor[m_RoomIndex].position;
            //}
            //else if (m_RoomIndex >= m_TotalNumberOfRoom)
            //{
            //    Destroy(gameObject);
            //}
        }
    }
    private void EnemySpawner()
    {
        for (int Index = 0; Index <= m_NumberOfEnemyToSpawn; Index++)
        {
            int RandomIndex = 0;
            RandomIndex = Random.Range(0, 2);
            Instantiate(m_EnemyPrefab[RandomIndex], m_EnemySpawnPoint[Index].position, Quaternion.identity);
        }
    }

    public void EnemyDied()
    {
        m_NumberOfEnemyToSpawn--;
        Debug.Log(m_NumberOfEnemyToSpawn);
        if (m_NumberOfEnemyToSpawn <= 0)
        {
            m_CanChangeRoom = true;
        }
    }

}
