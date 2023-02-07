using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eddy.Utilities
{
    public class LookAtCameraDirection : MonoBehaviour
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
            transform.rotation = Quaternion.LookRotation(-targetToLook.forward, Vector3.up);
        }
    }
}
