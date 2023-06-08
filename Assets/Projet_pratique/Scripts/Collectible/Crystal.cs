using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private Rigidbody2D m_RigidBody2D;
    private void Start()
    {
        int randomForce = Random.Range(5, 9);
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_RigidBody2D.AddForce(Vector2.up * randomForce, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.CrystalAdded(1);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Crystal")
        {
            int RandomDirection = Random.Range(-1, 2);
            m_RigidBody2D.AddForce(new Vector2(RandomDirection, 0) * 1, ForceMode2D.Impulse);
        }
    }
}
