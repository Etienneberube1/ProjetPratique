using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyAI : MonoBehaviour
{

    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_Speed = 20f;
    [SerializeField] private float m_NextWayPointDistance = 2f;
    private Path m_path;
    private int m_CurrentWayPoint = 0;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
