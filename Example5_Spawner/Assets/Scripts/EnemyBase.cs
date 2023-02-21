using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected Transform target;
    protected float hp = 0;
    [SerializeField]
    protected float hpTotal = 100;
    private float timer = 0f;
    [SerializeField]
    private float timerTotal = 1f;

    protected NavMeshAgent nav;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        hp = hpTotal;
        target = GameManager.instance.player;
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        TimerTool();
    }

    private void TimerTool()
    {
        if (timer > timerTotal)
        {
            timer = 0;
            TimerContent();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    protected virtual void TimerContent()
    {
        print("Time's up");
    }

    public virtual void Damaged(float damage)
	{
        hp = Mathf.Max(0, hp - damage);
        if(hp == 0)
		{
            Death();
        }
    }
    protected virtual void Death()
    {
        SpawnerManager.instance.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
