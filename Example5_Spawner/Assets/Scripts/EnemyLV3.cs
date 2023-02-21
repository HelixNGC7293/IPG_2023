using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLV3 : EnemyBase
{
    bool secondChange = true;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void TimerContent()
    {
        //Recover health
        nav.SetDestination(target.position);
    }
    protected override void Death()
    {
		if (secondChange)
		{
            secondChange = false;
            hp = hpTotal;
            nav.speed *= 2;
        }
        else
        {
            SpawnerManager.instance.RemoveEnemy(this);
            Destroy(gameObject);
        }
    }
}
