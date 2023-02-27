using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBase
{
	//All methods and properties in an interface are automatically public and so when declaring them, the public keyword is not required.
	Quaternion Rotation { get; }
	void Attack();
	void Damaged(float damage);
	void Death();
}
