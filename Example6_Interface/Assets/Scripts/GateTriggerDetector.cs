using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTriggerDetector : MonoBehaviour
{
	[SerializeField]
	int gateID = 0;
    [SerializeField]
    GateController gateController;
	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			gateController.GateTriggered(gateID);
		}
	}
}
