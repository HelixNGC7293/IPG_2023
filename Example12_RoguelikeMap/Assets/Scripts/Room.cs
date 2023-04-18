using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class WallTypeStructure
{
    public GameObject[] typeObjects;
}
public class Room : MonoBehaviour
{
    public Transform[] spawnPoints;

    //L/R/U/D
    Room[] roomsConnected = new Room[4];
    Transform[] hallwaysConnected = new Transform[4];
    [HideInInspector]
    public bool[] isDoorAlwaysOpen = {false, false, false, false};
    [HideInInspector]
    public bool[] isDoorOpen = { false, false, false, false };
    [HideInInspector]
    public string[] doorPasswords = { "", "", "", "" };

    [HideInInspector]
    public List<string> emptyConnectedJoint = new List<string>(){ "L", "R", "U", "D" };

    [HideInInspector]
    public int pX;
    [HideInInspector]
    public int pY;

    [SerializeField]
    WallTypeStructure[] WallTypeStructures;


    [HideInInspector]
    public GameObject roomObject;
    [HideInInspector]
    public List<GameObject> roomDecos = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < isDoorOpen.Length; i++)
		{
            isDoorOpen[i] = isDoorAlwaysOpen[i];

        }
    }

    public void ConnectingRoom(Room roomToBeConnected, string roomDirection, bool alwaysOpen, string password)
	{
        int wallID = 0;
		if (roomDirection == "L")
		{
            wallID = 0;
        }
		else if (roomDirection == "R")
        {
            wallID = 1;
        }
		else if (roomDirection == "U")
        {
            wallID = 2;
        }
		else if (roomDirection == "D")
        {
            wallID = 3;
        }
        roomsConnected[wallID] = roomToBeConnected;
        isDoorAlwaysOpen[wallID] = alwaysOpen;
        doorPasswords[wallID] = password;
        displayWallTypeStructure(wallID);
        emptyConnectedJoint.Remove(roomDirection);

    }

    void displayWallTypeStructure(int wallID)
    {
        if (roomsConnected[wallID] != null)
        {
            if (isDoorAlwaysOpen[wallID])
            {
                WallTypeStructures[wallID].typeObjects[0].SetActive(false);
                WallTypeStructures[wallID].typeObjects[1].SetActive(false);
                WallTypeStructures[wallID].typeObjects[2].SetActive(true);
            }
            else if (!isDoorAlwaysOpen[wallID])
            {
                WallTypeStructures[wallID].typeObjects[0].SetActive(false);
                WallTypeStructures[wallID].typeObjects[1].SetActive(true);
                WallTypeStructures[wallID].typeObjects[2].SetActive(false);
            }
        }
        else
        {
            WallTypeStructures[wallID].typeObjects[0].SetActive(true);
            WallTypeStructures[wallID].typeObjects[1].SetActive(false);
            WallTypeStructures[wallID].typeObjects[2].SetActive(false);
        }
    }

    public string GetPassword(int wallID)
	{
        return doorPasswords[wallID];
	}

    public void UnlockDoor(int wallID)
	{
        isDoorAlwaysOpen[wallID] = true;
        displayWallTypeStructure(wallID);

        int otherSideDoorWallID = 0;
        if(wallID == 0)
		{
            otherSideDoorWallID = 1;
		}
        else if (wallID == 1)
        {
            otherSideDoorWallID = 0;
        }
        else if (wallID == 2)
        {
            otherSideDoorWallID = 3;
        }
        else if (wallID == 3)
        {
            otherSideDoorWallID = 2;
        }
        roomsConnected[wallID].isDoorAlwaysOpen[otherSideDoorWallID] = true;
        roomsConnected[wallID].displayWallTypeStructure(otherSideDoorWallID);
    }
}
