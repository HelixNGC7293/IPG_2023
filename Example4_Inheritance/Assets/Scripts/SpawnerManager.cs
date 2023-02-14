using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager instance;

    public GameObject[] prefab_Bullets;
    public EnemyBase[] prefab_Enemies;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnBullets(int id, Vector3 pos, Quaternion rot)
    {

    }
    public void SpawnEnemies(int id, Vector3 pos, Quaternion rot)
    {

    }
}
