using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    private NavGrid _grid;
    private int _diagonalWeight = 14;
    private int _horVerWeight = 10;

    private void Awake()
    {
        _grid = GetComponent<NavGrid>();
    }


    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = _grid.NodeFromWorldPoint(request.PathStart);
        Node targetNode = _grid.NodeFromWorldPoint(request.PathEnd);

        if (startNode.Walkable && targetNode.Walkable)
        {
            Heap<Node> openSet = new Heap<Node>(_grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode); // Adding StartNode to OpenSet

            // Main Loop
            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                // If Found Target Node then Exit Loop
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                // Checking Neighbours
                foreach (Node neighbour in _grid.GetNeighbours(currentNode))
                {
                    // Skip Neighbour Logic
                    if ((neighbour.Empty && !request.CountAir) || !neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    // Compare Weight Costs
                    int moveCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour) + neighbour.MovementPenalty;
                    if (moveCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = moveCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.ParentNode = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }


        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        callback(new PathResult(waypoints, pathSuccess, request.Callback));
    }

    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.ParentNode;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    private Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 directionOld = Vector3.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 directionNew = new Vector3(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY, path[i - 1].GridZ - path[i].GridZ);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].WorldPosition);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);
        int distZ = Mathf.Abs(nodeA.GridZ - nodeB.GridZ);

        if (distX > distY)
        {
            return _diagonalWeight * distY + _horVerWeight * (distX - distY);
        }
        return _diagonalWeight * distX + _horVerWeight * (distY - distX);
    }
}
