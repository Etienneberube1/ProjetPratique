using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance => instance;
    private Player _player;
    public event Action<Player> PlayerData;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerRegister(Player player)
    {
        _player = player;
        PlayerData?.Invoke(_player);
    }
    public Player GetPlayer()
    {
        return _player;       
    }
}
