using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestSubModule : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestManager.instance.eventTest += LoadEventHandler;
    }

	private void OnDestroy()
    {
        TestManager.instance.eventTest -= LoadEventHandler;
    }

	public void LoadEventHandler()
	{
        print(name + " detected load data event!");
        TestManager.instance.eventTest -= LoadEventHandler;
    }
}
