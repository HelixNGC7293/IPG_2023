using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Maze2TunnelDirectionIndicator { None, Left, Right, Up, Down };
public class Maze2Cell : MonoBehaviour
{
    public Maze2TunnelDirectionIndicator tunnelDirection = Maze2TunnelDirectionIndicator.None;

    [HideInInspector]
    public bool isVisited = false;

    public float mazeSize = 5;

    public GameObject wall;
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
