using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathNode> gridSystem;
    private void Awake()
    {
        Instance = this;
    }
    public void Setup(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var gridPosition = new GridPosition(x, z);
                var worldPosition = GridLevel.Instance.GetWorldPosition(gridPosition);
                var raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }
    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);

        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                var gridPosition = new GridPosition(x, z);
                var pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            var currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                //Reached final node
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }
                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        pathLength = 0;
        return null; //no path found
    }
    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        var xDistance = Mathf.Abs(gridPositionDistance.x);
        var zDistance = Mathf.Abs(gridPositionDistance.z);
        var remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + remaining * MOVE_STRAIGHT_COST;
    }
    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        var lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }
    private PathNode GetNode(int x, int z) => gridSystem.GetGridObject(new GridPosition(x, z));
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        var gridPosition = currentNode.GetGridPosition();
        if (gridPosition.x - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                //Left Down
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));

            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Left Up
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
        }
        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                //Right Down
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Right Up
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }
        if (gridPosition.z - 1 >= 0)
        {
            //Down
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));

        }
        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            //Up
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        return neighbourList;
    }
    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }
    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable) => gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    public bool IsWalkableGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).IsWalkable();
    public bool HasPath(GridPosition startPos, GridPosition endPos) => FindPath(startPos, endPos, out int pathLength) != null;
    public int GetPathLength(GridPosition startPos, GridPosition endPos)
    {
        FindPath(startPos, endPos, out int pathLength);
        return pathLength;
    }
}
