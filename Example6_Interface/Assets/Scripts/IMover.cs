using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMover
{
	Vector3 Position { get; set; }
	void Move();
	void Remove();
}
