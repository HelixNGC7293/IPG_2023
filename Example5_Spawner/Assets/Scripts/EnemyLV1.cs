using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLV1 : EnemyBase
{
    protected override void TimerContent()
    {
        //Recover health
        nav.SetDestination(target.position);
    }

    public override void Damaged(float damage)
    {
        hp = Mathf.Max(0, hp - damage * 2);
        if (hp == 0)
        {
            Death();
        }
    }
}
