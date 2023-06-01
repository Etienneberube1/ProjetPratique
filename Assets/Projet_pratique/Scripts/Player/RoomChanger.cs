using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChanger : MonoBehaviour
{
    [SerializeField] private Transform[] m_RoomIndex;
    private int m_CurrentRoomIndex = 0;
    private int m_NumberOfRoom = 0;
    private void Start()
    {
        m_NumberOfRoom = m_RoomIndex.Length;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = m_RoomIndex[0].transform.position;
        }

    }
}
