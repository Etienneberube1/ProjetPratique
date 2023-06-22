using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngameDebugConsole;

public class DebugConsole : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private Transform BossRoomPos;
    private void Start()
    {
        DebugLogConsole.AddCommandInstance("BossRoom", "Will teleport gameobject to boss room", "BossRoomTP", this);
    }

    public void BossRoomTP()
    {
        Player.transform.position = BossRoomPos.transform.position;
    }
}
