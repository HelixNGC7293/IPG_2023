using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameEnding { DirectEscape, Explosion, Humanity }
public class GameManager : MonoBehaviour
{
	[HideInInspector]
	public bool isGameOver = false;

	float gameTimer = 30;
	[SerializeField]
	float gameTimer_Init = 30;
	[SerializeField]
	Transform gameTimerBarFill;
	[SerializeField]
	TextMeshProUGUI gameTimerText;


	public int itemMaximumNum = 7;
	[SerializeField]
	int itemTypes = 7;
	[HideInInspector]
	public int itemCollected = 0;
	[SerializeField]
	int roomMaximumNum = 45;
	[SerializeField]
	int roomRandomConnectionMaximumNum = 16;
	[SerializeField]
	int decoMaximumNum = 80;

	[SerializeField]
	GameObject prefab_HallwayH;

	[SerializeField]
	GameObject prefab_HallwayV;
	[SerializeField]
	Item prefab_Item;
	[SerializeField]
	GameObject[] prefabs_Deco;

	[SerializeField]
	Room prefab_Room;

	[SerializeField]
	Transform roomGroup;

	List<Room> currentRooms = new List<Room>();
	Dictionary<string, Room> currentRoomsByPos = new Dictionary<string, Room>();
	List<GameObject> currentHallways = new List<GameObject>();
	List<Item> currentItems = new List<Item>();


	List<GameObject> currentDecos = new List<GameObject>();


	string[] passwordTemplate = { "BLOOM", "ENJOY", "UNITY", "GLORY", "HELLO", "HONOR", "HUMOR", "INNER", "LUCKY", "VVVVV", "LIGHT", "LOGIC", "MERCY", "TOUCH", "TRUTH", "FORCE", "VIGOR", "EIGHT", "ETHER", "FUZZY", "CURLY", "ONION", "CLOVE", "PUPPY", "FRUIT", "CHUNK", "BEEFY" };

	// Use this for initialization
	void Start ()
	{
		//Init
		gameTimer = gameTimer_Init;

		GenerateRooms();
		GenerateDecos();
		GenerateItems();
	}

	void GenerateRooms()
	{
		int oX = 0;
		int oY = 0;
		int nextX = oX;
		int nextY = oY;
		Vector3 roomPos = new Vector3(nextX, nextY, 0);

		//Create root room
		Room createdRoom = Instantiate(prefab_Room, roomGroup);
		createdRoom.transform.position = roomPos;
		createdRoom.emptyConnectedJoint.Remove("R");
		createdRoom.pX = nextX;
		createdRoom.pY = nextY;
		currentRooms.Add(createdRoom);
		currentRoomsByPos.Add(nextX + "_" + nextY, createdRoom);

		for (int i = 0; i < roomMaximumNum; i++)
		{
			//Expand room
			Room selectedRoom = currentRooms[Random.Range(0, currentRooms.Count)];
			if (selectedRoom.emptyConnectedJoint.Count > 0)
			{
				string selectedDirection = selectedRoom.emptyConnectedJoint[Random.Range(0, selectedRoom.emptyConnectedJoint.Count)];

				string createdRoomReversedDirection = "L";
				GameObject prefab_Hallway;
				Vector3 hallwayPos = Vector3.zero;

				if (selectedDirection == "L")
				{
					nextX = selectedRoom.pX - 1;
					nextY = selectedRoom.pY;
					roomPos = selectedRoom.spawnPoints[0].position;
					hallwayPos = selectedRoom.spawnPoints[4].position;
					prefab_Hallway = prefab_HallwayH;
					createdRoomReversedDirection = "R";
				}
				else if (selectedDirection == "R")
				{
					nextX = selectedRoom.pX + 1;
					nextY = selectedRoom.pY;
					roomPos = selectedRoom.spawnPoints[1].position;
					hallwayPos = selectedRoom.spawnPoints[5].position;
					prefab_Hallway = prefab_HallwayH;
					createdRoomReversedDirection = "L";
				}
				else if (selectedDirection == "U")
				{
					nextX = selectedRoom.pX;
					nextY = selectedRoom.pY + 1;
					roomPos = selectedRoom.spawnPoints[2].position;
					hallwayPos = selectedRoom.spawnPoints[6].position;
					prefab_Hallway = prefab_HallwayV;
					createdRoomReversedDirection = "D";
				}
				else if (selectedDirection == "D")
				{
					nextX = selectedRoom.pX;
					nextY = selectedRoom.pY - 1;
					roomPos = selectedRoom.spawnPoints[3].position;
					hallwayPos = selectedRoom.spawnPoints[7].position;
					prefab_Hallway = prefab_HallwayV;
					createdRoomReversedDirection = "U";
				}
				else
				{
					prefab_Hallway = prefab_HallwayV;
				}

				if (currentRoomsByPos.ContainsKey(nextX + "_" + nextY))
				{
					//Debug: This location already has a room
					i--;
					continue;
				}
				else
				{
					createdRoom = Instantiate(prefab_Room, roomGroup);
					createdRoom.transform.position = roomPos;
					if (nextX == 0)
					{
						//Right edge
						createdRoom.emptyConnectedJoint.Remove("R");
					}
					createdRoom.pX = nextX;
					createdRoom.pY = nextY;
					currentRooms.Add(createdRoom);
					currentRoomsByPos.Add(nextX + "_" + nextY, createdRoom);
					bool doorAlwaysOpen = true;
					if (Random.value < 0.35f)
					{
						doorAlwaysOpen = false;
					}

					string password = passwordTemplate[Random.Range(0, passwordTemplate.Length)];

					selectedRoom.ConnectingRoom(createdRoom, selectedDirection, doorAlwaysOpen, password);
					createdRoom.ConnectingRoom(selectedRoom, createdRoomReversedDirection, doorAlwaysOpen, password);


					GameObject hallway = Instantiate(prefab_Hallway, roomGroup);
					hallway.transform.position = hallwayPos;
					currentHallways.Add(hallway);
				}

			}
			else
			{
				i--;
				continue;
			}

		}
		//Add Random Hallways

		for (int i = 0; i < roomRandomConnectionMaximumNum; i++)
		{
			//Expand room
			Room selectedRoom = currentRooms[Random.Range(0, currentRooms.Count)];
			if (selectedRoom.emptyConnectedJoint.Count > 0)
			{
				string selectedDirection = selectedRoom.emptyConnectedJoint[Random.Range(0, selectedRoom.emptyConnectedJoint.Count)];

				string createdRoomReversedDirection = "L";
				GameObject prefab_Hallway;
				Vector3 hallwayPos = Vector3.zero;

				if (selectedDirection == "L")
				{
					nextX = selectedRoom.pX - 1;
					nextY = selectedRoom.pY;
					hallwayPos = selectedRoom.spawnPoints[4].position;
					prefab_Hallway = prefab_HallwayH;
					createdRoomReversedDirection = "R";
				}
				else if (selectedDirection == "R")
				{
					nextX = selectedRoom.pX + 1;
					nextY = selectedRoom.pY;
					hallwayPos = selectedRoom.spawnPoints[5].position;
					prefab_Hallway = prefab_HallwayH;
					createdRoomReversedDirection = "L";
				}
				else if (selectedDirection == "U")
				{
					nextX = selectedRoom.pX;
					nextY = selectedRoom.pY + 1;
					hallwayPos = selectedRoom.spawnPoints[6].position;
					prefab_Hallway = prefab_HallwayV;
					createdRoomReversedDirection = "D";
				}
				else if (selectedDirection == "D")
				{
					nextX = selectedRoom.pX;
					nextY = selectedRoom.pY - 1;
					hallwayPos = selectedRoom.spawnPoints[7].position;
					prefab_Hallway = prefab_HallwayV;
					createdRoomReversedDirection = "U";
				}
				else
				{
					prefab_Hallway = prefab_HallwayV;
				}

				if (currentRoomsByPos.ContainsKey(nextX + "_" + nextY))
				{
					//Found exist room and create hallway
					createdRoom = currentRoomsByPos[nextX + "_" + nextY];
					bool doorAlwaysOpen = true;
					if (Random.value < 0.35f)
					{
						doorAlwaysOpen = false;
					}

					string password = passwordTemplate[Random.Range(0, passwordTemplate.Length)];

					selectedRoom.ConnectingRoom(createdRoom, selectedDirection, doorAlwaysOpen, password);
					createdRoom.ConnectingRoom(selectedRoom, createdRoomReversedDirection, doorAlwaysOpen, password);


					GameObject hallway = Instantiate(prefab_Hallway, roomGroup);
					hallway.transform.position = hallwayPos;
					currentHallways.Add(hallway);
				}
			}
			else
			{
				i--;
				continue;
			}

		}

		//Remove Room 0 for empty init space
		currentRooms.RemoveAt(0);
	}

	void GenerateItems()
	{
		List<int> itemsList = new List<int>();
		for (int i = 0; i< itemTypes; i++)
		{
			itemsList.Add(i);
		}

		for (int i = 0; i < itemMaximumNum; i++)
		{
			Room selectedRoom = currentRooms[0];
			bool foundRoomWithEmptyObject = false;
			int maximumTry = 0;
			while (!foundRoomWithEmptyObject || maximumTry > 10000)
			{
				selectedRoom = currentRooms[Random.Range(0, currentRooms.Count)];
				if(selectedRoom.roomObject == null)
				{
					foundRoomWithEmptyObject = true;
					break;
				}
				maximumTry++;
			}

			if (maximumTry > 10000)
			{
				return;
			}
			int itemID = itemsList[Random.Range(0, itemsList.Count)];
			itemsList.Remove(itemID);

			Item createdItem = Instantiate(prefab_Item, roomGroup);
			createdItem.Init(itemID);
			createdItem.transform.position = selectedRoom.transform.position + new Vector3(Random.Range(-1.001f, 1.001f), Random.Range(-1.001f, 1.001f), 0);
			currentItems.Add(createdItem);
			selectedRoom.roomObject = createdItem.gameObject;
		}
	}


	void GenerateDecos()
	{

		for (int i = 0; i < decoMaximumNum; i++)
		{
			Room selectedRoom = currentRooms[0];
			bool foundRoomWithEmptyObject = false;
			int maximumTry = 0;
			while (!foundRoomWithEmptyObject || maximumTry > 10000)
			{
				selectedRoom = currentRooms[Random.Range(0, currentRooms.Count)];
				if (selectedRoom.roomDecos.Count < 6)
				{
					foundRoomWithEmptyObject = true;
					break;
				}
				maximumTry++;
			}

			if(maximumTry > 10000)
			{
				return;
			}
			GameObject prefab = prefabs_Deco[Random.Range(0, prefabs_Deco.Length)];

			GameObject created = Instantiate(prefab, roomGroup);
			created.transform.position = selectedRoom.transform.position + new Vector3(Random.Range(-1.501f, 1.501f), Random.Range(-1.501f, 1.501f), 0);
			currentDecos.Add(created);
			selectedRoom.roomDecos.Add(created);
		}
	}

	void Update()
	{
		if (!isGameOver)
		{
			if(gameTimer <= 0)
			{
				//Exploded
				gameTimer = 0;
				GameOver(GameEnding.Explosion);
			}
			else
			{
				gameTimer -= Time.deltaTime;
			}
			//gameTimerBarFill.localScale = new Vector3(Mathf.Min(gameTimer / gameTimer_Init, 1), 1, 1);
			//gameTimerText.text = "Self Destruction: \n<c=tak><w>" + Mathf.RoundToInt(gameTimer) + "s";
		}
	}


	public void GameOver(GameEnding gameEnding)
	{
		if (!isGameOver)
		{
			string nextScene = "WinState";

			isGameOver = true;

			//Play death animation
			SceneManager.LoadScene(nextScene);
		}
	}


}
