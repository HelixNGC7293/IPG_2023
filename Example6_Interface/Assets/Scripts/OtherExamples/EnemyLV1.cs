using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLV1 : MonoBehaviour, IEnemyBase
{
    float startingHealth = 100f;
    float m_CurrentHealth;
    public Quaternion Rotation
    {
        get
        {
            return transform.rotation;
        }
    }

    void Start()
    {
        m_CurrentHealth = startingHealth;
    }

    public void Damaged(float damage)
    {
        m_CurrentHealth -= damage;
    }

    public void Attack()
    {
        //Do something
        print("Attack");
    }

    public void Death()
    {
        //Do something
        print("Death");
    }
}
