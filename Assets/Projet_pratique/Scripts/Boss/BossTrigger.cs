using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private BossBattle m_BossBattleScript;
    void Start()
    {
        m_BossBattleScript = GetComponentInParent<BossBattle>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_BossBattleScript.StartBattle();
            Destroy(gameObject);
        }
    }
}
