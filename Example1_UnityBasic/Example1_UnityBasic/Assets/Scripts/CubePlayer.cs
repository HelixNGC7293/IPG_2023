using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlayer : MonoBehaviour
{
	[SerializeField]
	private GameManager gameManager;

	private Vector3 currentTarget;

	private void Start()
	{
		currentTarget = Vector3.up * 0.5f;
	}
	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.CompareTag("Coin"))
		{
			Destroy(collision.gameObject);
			gameManager.score++;
		}
	}

	/// <summary>
	/// Move to this location
	/// </summary>
	/// <param name="newTarget">Add new target position</param>
	public void MoveTo(Vector3 newTarget)
	{
		currentTarget = newTarget + Vector3.up * 0.5f;
	}

	private void Update()
	{
		Vector3 newPos = Vector3.Lerp(transform.position, currentTarget, 0.01f);
		Vector3 moveDirection = newPos - transform.position;
		moveDirection.y = 0;
		transform.position = newPos;

		transform.rotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);
	}
}
