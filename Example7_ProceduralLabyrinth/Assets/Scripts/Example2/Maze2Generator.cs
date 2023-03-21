using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze2Generator : MonoBehaviour
{
    [SerializeField]
    PrefabDatabase prefabDB;
    [SerializeField]
    int mazeX = 59;
    [SerializeField]
    int mazeY = 59;
    [SerializeField]
    Transform mazeGroup;

    Maze2Cell[,] mazeCellMap;

    List<Maze2Cell> unvisitCells = new List<Maze2Cell>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        mazeCellMap = new Maze2Cell[mazeX, mazeY];
        //Build all cells
        for (int x = 0; x < mazeX; x++)
        {
            for (int y = 0; y < mazeY; y++)
            {
                Maze2Cell cell = Instantiate(prefabDB.prefabList[1], mazeGroup).GetComponent<Maze2Cell>();
                cell.transform.position = new Vector3(cell.mazeSize * x, 0, cell.mazeSize * y);

                mazeCellMap[x, y] = cell;

                //Assign the current position to cell
                cell.Init(x, y);
            }
        }

        Maze2Cell startCell = mazeCellMap[1, 1];
        unvisitCells.Add(startCell);
        RecursiveBacktracking(startCell);
    }

    List<Maze2Cell> CheckCellSurroundings(Maze2Cell cell)
    {
        List<Maze2Cell> neighborUnvisitedCells = new List<Maze2Cell>();

        if (cell.locX - 2 > 0)
        {
            Maze2Cell checkingNeighborCell = mazeCellMap[cell.locX - 2, cell.locY];
            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
                checkingNeighborCell.tunnelDirection = Maze2TunnelDirectionIndicator.Left;
            }
        }
        if (cell.locX + 2 < mazeX - 1)
        {
            Maze2Cell checkingNeighborCell = mazeCellMap[cell.locX + 2, cell.locY];
            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
                checkingNeighborCell.tunnelDirection = Maze2TunnelDirectionIndicator.Right;
            }
        }
        if (cell.locY - 2 > 0)
        {
            Maze2Cell checkingNeighborCell = mazeCellMap[cell.locX, cell.locY - 2];
            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
                checkingNeighborCell.tunnelDirection = Maze2TunnelDirectionIndicator.Up;
            }
        }
        if (cell.locY + 2 < mazeY - 1)
        {
            Maze2Cell checkingNeighborCell = mazeCellMap[cell.locX, cell.locY + 2];
            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
                checkingNeighborCell.tunnelDirection = Maze2TunnelDirectionIndicator.Down;
            }
        }

        return neighborUnvisitedCells;
    }

    void RecursiveRandomPrim(Maze2Cell startCell)
    {
        unvisitCells.Remove(startCell);
        if (!startCell.isVisited)
        {
            startCell.isVisited = true;
            startCell.wall.SetActive(false);

            //Instant relink from main path
            if (startCell.tunnelDirection == Maze2TunnelDirectionIndicator.Right)
            {
                mazeCellMap[startCell.locX - 1, startCell.locY].wall.SetActive(false);
            }
            else if (startCell.tunnelDirection == Maze2TunnelDirectionIndicator.Left)
            {
                mazeCellMap[startCell.locX + 1, startCell.locY].wall.SetActive(false);
            }
            else if (startCell.tunnelDirection == Maze2TunnelDirectionIndicator.Up)
            {
                mazeCellMap[startCell.locX, startCell.locY + 1].wall.SetActive(false);
            }
            else if (startCell.tunnelDirection == Maze2TunnelDirectionIndicator.Down)
            {
                mazeCellMap[startCell.locX, startCell.locY - 1].wall.SetActive(false);
            }

            //Standard connection

            List<Maze2Cell> neighborUnvisitedCells = CheckCellSurroundings(startCell);


            if (neighborUnvisitedCells.Count > 0)
            {
                //Connect to the nearby unvisit cell
                Maze2Cell endCell = neighborUnvisitedCells[Random.Range(0, neighborUnvisitedCells.Count)];
                endCell.isVisited = true;
                endCell.wall.SetActive(false);

                if (endCell.locX < startCell.locX)
                {
                    mazeCellMap[startCell.locX - 1, startCell.locY].wall.SetActive(false);
                }
                else if (endCell.locX > startCell.locX)
                {
                    mazeCellMap[startCell.locX + 1, startCell.locY].wall.SetActive(false);
                }
                else if (endCell.locY < startCell.locY)
                {
                    mazeCellMap[startCell.locX, startCell.locY - 1].wall.SetActive(false);
                }
                else if (endCell.locY > startCell.locY)
                {
                    mazeCellMap[startCell.locX, startCell.locY + 1].wall.SetActive(false);
                }

                //Remove visited endCell from unvisited cell list
                neighborUnvisitedCells.Remove(endCell);

                //Get all unvisited cells around startCell & endCell
                unvisitCells.AddRange(neighborUnvisitedCells);
                unvisitCells.AddRange(CheckCellSurroundings(endCell));

            }
        }
        if (unvisitCells.Count > 0)
        {
            //As long as there is cell in the list
            //Randomly choose one cell and continue
            RecursiveRandomPrim(unvisitCells[Random.Range(0, unvisitCells.Count)]);
        }

    }
}
