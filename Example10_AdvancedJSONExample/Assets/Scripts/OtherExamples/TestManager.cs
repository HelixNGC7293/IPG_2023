using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase
{
    public string name;
    public int hp;
    public int atk;
    public int def;
}

public class TestManager : MonoBehaviour
{
    public delegate void EventTest();
    public EventTest eventTest;

    public static TestManager instance;

    bool isLoaded = false;
    private void Awake()
    {
		if (instance == null)
		{
            instance = this;
		}
    }

	// Start is called before the first frame update
	void Update()
    {
        if(Random.value < 0.001f && !isLoaded)
		{
            //Player steps into this dungeon room, read data and spawn enemy
            LoadData();
        }
    }

    public void LoadData()
	{
        isLoaded = true;
        //Load enemy1's data
        TextAsset txtAsset = Resources.Load<TextAsset>("enemyData/enemyData1");
        string textContent = txtAsset.text;
        print(textContent);
        string[] data = textContent.Split("##");
        EnemyBase enemy = new EnemyBase();
        enemy.name = data[0];
        enemy.hp = int.Parse(data[1]);
        enemy.atk = int.Parse(data[2]);
        enemy.def = int.Parse(data[3]);

        print("name: " + enemy.name);
        print("hp: " + enemy.hp);
        print("atk: " + enemy.atk);
        print("def: " + enemy.def);

        eventTest?.Invoke();
    }

}
