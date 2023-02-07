using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Eddy.CharacterPack
{
	public class Cosplayer : MonoBehaviour
	{
		//ImageExample imageExample;
		[SerializeField]
		Transform displayGroup;
		[SerializeField]
		Transform hungryBar;

		float _hungryValue = 0;
		float HungryValue
		{
			get { return _hungryValue; }
			set { _hungryValue = Mathf.Max(0, value); }
		}
		float HungryValueTotal = 1000;
		bool isDying = false;
		Transform restaurant;

		[SerializeField]
		Transform mainBody;
		[SerializeField]
		Animator anim;

		Transform mainCamera;

		NavMeshAgent nav;

		string currentDirection = "Right";
		float timer_Direction = 0;
		float timer_DirectionTotal = 1;


		float moveTimer = 0;
		float moveTimerTotal = 0;

		// Use this for initialization
		void Start()
		{
			mainCamera = Camera.main.transform;

			HungryValue = HungryValueTotal;
			nav = GetComponent<NavMeshAgent>();
			restaurant = GameObject.Find("Food_MaidCoffee").transform;

			ResetMoveTimer();
		}

		void ResetMoveTimer()
		{
			moveTimer = 0;
			moveTimerTotal = Random.Range(1.1f, 3.3f);
		}

		// Update is called once per frame
		void Update()
		{
			if (!isDying)
			{
				if (HungryValue > 0)
				{
					HungryValue -= 10f * Time.deltaTime;
					//Display UI
					if (HungryValue < HungryValueTotal / 2)
					{
						displayGroup.localScale = Vector3.one;
						hungryBar.transform.localScale = new Vector3(HungryValue / (HungryValueTotal / 2), 1, 1);
					}
					else
					{
						displayGroup.localScale = Vector3.zero;
					}

					if (moveTimer > moveTimerTotal)
					{
						ResetMoveTimer();
						nav.isStopped = false;

						if (HungryValue < HungryValueTotal / 2)
						{
							//Run to the restaurant
							nav.SetDestination(restaurant.position);
							nav.speed = 8;
						}
						else
						{
							nav.SetDestination(transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
							nav.speed = 3;
						}
					}
					else
					{
						moveTimer += Time.deltaTime;
					}
				}
				else
				{
					displayGroup.gameObject.SetActive(false);
					isDying = true;
					anim.SetTrigger("Dying");
					nav.isStopped = true;
				}
			}


			if (nav.velocity.magnitude < 0.1f)
			{
				anim.SetBool("isWalking", false);
			}
			else
			{
				anim.SetBool("isWalking", true);
				anim.SetFloat("Velocity", nav.velocity.magnitude);
			}

			//Direction Change Detection
			if (timer_Direction > timer_DirectionTotal)
			{
				timer_Direction = 0;
				timer_DirectionTotal = Random.Range(0.2f, 1.23f);
				float currentRelativeDirection = Vector3.Dot(nav.velocity, mainCamera.right);
				if (currentRelativeDirection > 0 && currentDirection == "Right")
				{
					currentDirection = "Left";
					var localS = mainBody.localScale;
					localS.x *= -1;
					mainBody.localScale = localS;
				}
				else if (currentRelativeDirection < 0 && currentDirection == "Left")
				{
					currentDirection = "Right";
					var localS = mainBody.localScale;
					localS.x *= -1;
					mainBody.localScale = localS;
				}
			}
			else
			{
				timer_Direction += Time.deltaTime;
			}
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.CompareTag("Booth_Food") && HungryValue < HungryValueTotal / 2)
			{
				//Eat food at the booth
				anim.SetTrigger("Buy");
				ResetMoveTimer();
				nav.isStopped = true;

				//Recover health
				HungryValue = HungryValueTotal;
			}
		}
	}
}
