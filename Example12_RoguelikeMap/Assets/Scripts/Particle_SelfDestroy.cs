using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_SelfDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject, GetComponent<ParticleSystem>().main.duration + 2);
	}
}
