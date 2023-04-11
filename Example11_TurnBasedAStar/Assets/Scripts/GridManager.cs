using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AStarNode
{
    public GridUnit GridUnit;
    public AStarNode Parent;
    public int GCost;
    public int HCost;
    public int FCost => GCost + HCost;
}
public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    [SerializeField]
    GridUnit prefab_GridUnit;
    [SerializeField]
    Transform mapGroup;
    [SerializeField]
    Character character;

    List<GridUnit> gridUnitList = new List<GridUnit>();

    [HideInInspector]
    public bool inMovingMode = false;
    [HideInInspector]
    public GridUnit currentAvailableMoveTarget;

    int[,] mapData = {
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 1, 0, 0, 0, 0, 2, 0, 0, 0 },
    { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };

	private void Awake()
	{
		if(instance == null)
		{
            instance = this;
		}
	}
	// Start is called before the first frame update
	void Start()
    {
        GenerateMap();

        character.currentLocation = gridUnitList[35];
        print(character.currentLocation.x + " " + character.currentLocation.y);
    }

    void GenerateMap()
	{
        int mapX = mapData.GetLength(1);
        int mapY = mapData.GetLength(0);
        float centerX = 0.5f - (float)mapX / 2;
        float centerY = 0.5f - (float)mapY / 2;

        for (int y = 0; y < mapY; y++)
        {
            for (int x = 0; x < mapX; x++)
            {
                GridUnit unit = Instantiate(prefab_GridUnit, mapGroup);
                unit.Init(x, y, mapData[y, x]);
                unit.transform.position = new Vector2(x + centerX, -y - centerY);

                gridUnitList.Add(unit);
            }
        }
	}

    public void SetTargetLocation(GridUnit targetLocation)
	{
        CalculatePathfinding(character.currentLocation, targetLocation, character.actionPoint);
    }

    int GetCost(int tileType)
	{
        int cost = 0;
        switch (tileType)
        {
            case 0:
                cost = 1;
                break;
            case 1:
                cost = 2;
                break;
            case 2:
                cost = 99;
                break;
        }

        return cost;
    }

    private int GetDistance(GridUnit a, GridUnit b)
    {
        int distX = Mathf.Abs(a.x - b.x);
        int distY = Mathf.Abs(a.y - b.y);

        if (distX > distY)
        {
            return 10 * distY + 10 * (distX - distY);
        }
        return 10 * distX + 10 * (distY - distX);
    }
    private List<GridUnit> GetNeighbors(GridUnit gridUnit)
    {
        List<GridUnit> neighbors = new List<GridUnit>();
        int mapX = mapData.GetLength(1);
        int mapY = mapData.GetLength(0);

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //Diagonal grids are not neighbor
                if (Mathf.Abs(x) == Mathf.Abs(y)) continue;

                int checkX = gridUnit.x + x;
                int checkY = gridUnit.y + y;

                if (checkX >= 0 && checkX < mapX && checkY >= 0 && checkY < mapY)
                {
                    neighbors.Add(gridUnitList[checkY * mapX + checkX]);
                }
            }
        }

        return neighbors;
    }
    public List<GridUnit> CalculateReachableGrids(GridUnit startLocation, int actionPoint)
    {
        List<GridUnit> reachableGrids = new List<GridUnit>();
        //First in first out
        Queue<(GridUnit, int)> queue = new Queue<(GridUnit, int)>();
        //HashSet is a simple collection without orders
        HashSet<GridUnit> visited = new HashSet<GridUnit>();

        queue.Enqueue((startLocation, 0));
        visited.Add(startLocation);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            GridUnit gridUnit = current.Item1;
            int cost = current.Item2;

            if (cost <= actionPoint)
            {
                reachableGrids.Add(gridUnit);

                foreach (GridUnit neighbor in GetNeighbors(gridUnit))
                {
                    int newCost = cost + GetCost(neighbor.tileType);

                    if (!visited.Contains(neighbor) && newCost <= actionPoint && neighbor.tileType != 2)
                    {
                        queue.Enqueue((neighbor, newCost));
                        visited.Add(neighbor);
                    }
                }
            }
        }

        return reachableGrids;
    }

    public void CalculatePathfinding(GridUnit startLocation, GridUnit targetLocation, int actionPoint)
    { 
        // Create the open list containing the start node and closed set
        List<AStarNode> openList = new List<AStarNode>();
        HashSet<GridUnit> closedSet = new HashSet<GridUnit>();
        Dictionary<GridUnit, AStarNode> nodeLookup = new Dictionary<GridUnit, AStarNode>();

        AStarNode startNode = new AStarNode { GridUnit = startLocation };
        openList.Add(startNode);
        nodeLookup[startLocation] = startNode;
        // Main loop, continues until there are no more nodes to explore
        while (openList.Count > 0)
        {
            // Find the node with the lowest F cost
            AStarNode currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost)
                {
                    currentNode = openList[i];
                }
            }
            // Remove the current node from the open list and add it to the closed set
            openList.Remove(currentNode);
            closedSet.Add(currentNode.GridUnit);
            // Check if the current node is the target location, if so, retrace the path
            if (currentNode.GridUnit == targetLocation)
            {
                RetracePath(startNode, currentNode, actionPoint);
                return;
            }
            // Explore neighbors of the current node
            foreach (GridUnit neighbor in GetNeighbors(currentNode.GridUnit))
            { 
                // Ignore blocked nodes or nodes already in the closed set
                if (neighbor.tileType == 2 || closedSet.Contains(neighbor))
                {
                    continue;
                }
                // Calculate the new G cost
                int newGCost = currentNode.GCost + GetDistance(currentNode.GridUnit, neighbor);
                AStarNode neighborNode;
                if (!nodeLookup.ContainsKey(neighbor))
                {
                    neighborNode = new AStarNode { GridUnit = neighbor };
                    nodeLookup[neighbor] = neighborNode;
                }
                else
                {
                    neighborNode = nodeLookup[neighbor];
                }
                // Update neighbor's G, H, and Parent if the new G cost is lower, or if the neighbor is not in the open list
                if (newGCost < neighborNode.GCost || !openList.Contains(neighborNode))
                {
                    neighborNode.GCost = newGCost;
                    neighborNode.HCost = GetDistance(neighbor, targetLocation);
                    neighborNode.Parent = currentNode;
                    // Add neighbor to the open list if it's not already there
                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
    }

    public void StartMovingMode()
    {
        inMovingMode = true;
        List<GridUnit> reachableGrids = CalculateReachableGrids(character.currentLocation, character.actionPoint);
        foreach (GridUnit gridUnit in reachableGrids)
        {
            gridUnit.DisplayReachableGrid();
        }
    }

    public void StopMovingMode()
    {
        inMovingMode = false;
        foreach(GridUnit grid in gridUnitList)
		{
            grid.ResetGrid();
		}
    }

    private void RetracePath(AStarNode startNode, AStarNode endNode, int actionPoint)
    {
        // Create an empty list to store the path
        List<GridUnit> path = new List<GridUnit>();
        AStarNode currentNode = endNode;
        // Follow parent pointers from the end node to the start node, adding each node to the path
        while (currentNode != startNode)
        {
            path.Add(currentNode.GridUnit);
            currentNode = currentNode.Parent;
        }
        // Reverse the path so it starts from the starting location
        path.Reverse();
        // Hide the pathfinding visualization for all grid units
        foreach (GridUnit gridUnit in gridUnitList)
        {
            gridUnit.HidePathfinding();
        }
        // Display the path and calculate its cost
        int pathCost = 0;
        foreach (GridUnit gridUnit in path)
        {
            pathCost += GetCost(gridUnit.tileType);
            // If the path cost exceeds the action points available, stop displaying the path
            if (pathCost > actionPoint)
            {
                break;
            }
            // Update the current available move target and display the pathfinding visualization with the path cost
            currentAvailableMoveTarget = gridUnit;
            gridUnit.DisplayPathfinding(pathCost.ToString());
        }
    }

    public void ConfirmMovement(GridUnit clickedTargetGrid = null)
	{
        if(clickedTargetGrid != null)
        {
            character.currentLocation = clickedTargetGrid;
        }
        else if(currentAvailableMoveTarget != null)
        {
            character.currentLocation = currentAvailableMoveTarget;
        }

        character.transform.position = character.currentLocation.transform.position;
        StopMovingMode();
    }
}
