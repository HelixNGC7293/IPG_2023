using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class PlayerUnit : MonoBehaviour
{
    Rigidbody rB;
    float speed = 1000;
    // Start is called before the first frame update
    void Start()
    {
        rB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        pos.y = 0.3f;
        transform.position = pos;
        //if (transform.position.y > 0.9f)
        //{
        //}

		var direction = -transform.localPosition;
        var newVel = direction * speed * Time.deltaTime;
        newVel.y = 0;
		if (newVel.sqrMagnitude > 1)
		{
            newVel = newVel.normalized * 1;
        }
        newVel.y = rB.velocity.y;

        rB.velocity = newVel;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("DeathZone"))
    //    {
    //        GameManager.instance.RemoveUnit(this);
    //    }
    //}

}
