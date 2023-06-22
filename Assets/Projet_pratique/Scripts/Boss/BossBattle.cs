using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class BossBattle : MonoBehaviour
{
    public enum Stage
    {
        WaitingToStart,
        Stage_1,
        Stage_2,
        Stage_3
    }
    [SerializeField] private GameObject m_RollerPrebab;
    [SerializeField] private Pig m_PigBoss;
    [SerializeField] private BossShield m_BossShieldScript;
    public bool BossBattleTrigger = false;
    private List<Vector3> m_SpawnPointList;
    private List<GameObject> m_EnemySpawnList;
    public Stage m_Stage;

    private void Awake()
    {
        m_Stage = Stage.WaitingToStart;
        m_SpawnPointList = new List<Vector3>();
        m_EnemySpawnList = new List<GameObject>();

        foreach (Transform spawnPosition in transform.Find("SpawnPositions"))
        {
            m_SpawnPointList.Add(spawnPosition.position);
        }

    }
    private void Update()
    {
        if (m_Stage != Stage.WaitingToStart)
        {
            Attack();
        }
    }
    public void StartBattle()
    {
        SpawnEnemy();
        StartNextStage();
        FunctionPeriodic.Create(SpawnEnemy, 3f, "SpawnEnemy");
    }
    public void Attack()
    {
        if (m_PigBoss != null)
        {
            
            m_PigBoss.StartAttack();
        }
    }
    public void BossBattle_OnDamaged()
    {
        switch (m_Stage)
        {
            case Stage.Stage_1:
                if (m_PigBoss.m_enemyHP <= 210)
                {
                    // boss is under 70% hp
                    StartNextStage();
                    m_BossShieldScript.isShieldActive = true;
                }
                break;
            case Stage.Stage_2:
                if (m_PigBoss.m_enemyHP <= 100)
                {
                    // boss is under 40% hp
                    StartNextStage();
                }
                break;
            case Stage.Stage_3:
                if (m_PigBoss.m_enemyHP <= 0)
                {
                    BattleIsOver();
                }
                break;
        }
    }
    private void BattleIsOver()
    {
        DestroyAllEnemy();
        FunctionPeriodic.StopAllFunc("SpawnEnemy");
        m_PigBoss.DestroyPig();
    }
    private void StartNextStage()
    {
        switch (m_Stage)
        {
            case Stage.WaitingToStart:
                m_Stage = Stage.Stage_1;
                break;
            case Stage.Stage_1:
                m_Stage = Stage.Stage_2;
                break;
            case Stage.Stage_2:
                m_Stage = Stage.Stage_3;
                break;
        }
        Debug.Log("starting stage: " + m_Stage);
    }
    private void SpawnEnemy()
    {
        Vector3 spawnPosition = m_SpawnPointList[Random.Range(0, m_SpawnPointList.Count)];
        GameObject enemy = Instantiate(m_RollerPrebab, spawnPosition, Quaternion.identity);
        m_EnemySpawnList.Add(enemy);
    }
    private void DestroyAllEnemy()
    {
        foreach (GameObject enemy in m_EnemySpawnList)
        {
            Destroy(enemy);
        }
    }
}
