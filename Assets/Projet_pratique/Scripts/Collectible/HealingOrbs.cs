using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrbs : MonoBehaviour
{
    [SerializeField] private int HealAmmount = 10;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Player player = col.gameObject.GetComponent<Player>();
            player.Heal(HealAmmount);
            Destroy(gameObject);
        }
    }
}
