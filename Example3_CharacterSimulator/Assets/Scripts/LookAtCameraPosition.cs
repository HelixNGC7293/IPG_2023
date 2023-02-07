using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eddy.Utilities
{
	public class LookAtCameraPosition : MonoBehaviour
	{
		Transform targetToLook;
		// Use this for initialization
		void Start()
		{
			targetToLook = Camera.main.transform;
		}

		// Update is called once per frame
		void Update()
		{
			transform.LookAt(targetToLook);
		}
	}
}
