using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlayer : MonoBehaviour
{
	public enum MovingPatterns { Straight, Lerp, Teleport, EaseIn, EaseInAndOut }
	[SerializeField]
	private MovingPatterns movingPatterns;
	[SerializeField]
	private GameManager gameManager;

	private Vector3 currentTarget;
	private Vector3 startPos;
	private float movementTimer = 0;

	private void Start()
	{
		currentTarget = Vector3.up * 0.5f;
	}
	private void OnCollisionEnter(Collision collision)
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
		startPos = transform.position;
		currentTarget = newTarget + Vector3.up * 0.5f;

		//Extra: For Ease Movement Mode
		movementTimer = 0;
	}

	private void Update()
	{
		//Default moving direction
		Vector3 targetDistance = currentTarget - transform.position;
		targetDistance.y = 0;
		Vector3 moveDirection = targetDistance.normalized;

		Vector3 newPos = Vector3.zero;
		if (movingPatterns == MovingPatterns.Teleport)
		{
			newPos = currentTarget;
		}
		else if (movingPatterns == MovingPatterns.Straight)
		{
			Vector3 moveVector = moveDirection * 10f * Time.deltaTime;
			if (moveVector.sqrMagnitude > targetDistance.sqrMagnitude)
			{
				//Reached target
				newPos = currentTarget;
			}
			else
			{
				//Keep moving in average speed
				newPos = transform.position + moveVector;
			}
		}
		else if (movingPatterns == MovingPatterns.Lerp)
		{
			//Lerp: a + (b - a) * t
			newPos = Vector3.Lerp(transform.position, currentTarget, 2f * Time.deltaTime);
		}
		else if (movingPatterns == MovingPatterns.EaseIn)
		{
			//Extra: Using easing algorithm from https://easings.net/
			movementTimer = Mathf.Min(1, movementTimer + Time.deltaTime);
			newPos = startPos + (currentTarget - startPos) * (1 - Mathf.Cos((movementTimer * Mathf.PI) / 2));
		}
		else if (movingPatterns == MovingPatterns.EaseInAndOut)
		{
			//Extra: Using easing algorithm from https://easings.net/
			movementTimer = Mathf.Min(1, movementTimer + Time.deltaTime);
			newPos = startPos + (currentTarget - startPos) * -(Mathf.Cos(Mathf.PI * movementTimer) - 1) / 2;
		}



		transform.position = newPos;
		if (moveDirection != Vector3.zero)
		{
			transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
		}
	}
}
