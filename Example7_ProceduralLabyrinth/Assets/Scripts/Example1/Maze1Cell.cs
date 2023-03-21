using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze1Cell : MonoBehaviour
{

    [HideInInspector]
    public bool isVisited = false;

    public float mazeSize = 5;
    public GameObject[] walls;
    //0=L, 1=R, 2=U, 3=D
    //[HideInInspector]
    //public List<int> existWalls = new List<int>() { 0, 1, 2, 3 };
    [HideInInspector]
    public int locX;
    [HideInInspector]
    public int locY;

	public void Init(int x, int y)
	{
        locX = x;
        locY = y;
    }
}
