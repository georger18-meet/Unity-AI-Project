using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;

    private NavGrid _grid;
    private int _diagonalWeight = 14;
    private int _horVerWeight = 10;

    private void Awake()
    {
        _grid = GetComponent<NavGrid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = _grid.NodeFromWorldPoint(startPos);
        Node targetNode = _grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode); // Adding StartNode to OpenSet

        // Main Loop
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // If Found Target Node then Exit Loop
            if (currentNode == targetNode) 
            {
                RetracePath(startNode, targetNode);
                return; 
            }

            // Checking Neighbours
            foreach (Node neighbour in _grid.GetNeighbours(currentNode))
            {
                // Skip Neighbour Logic
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                // Compare Weight Costs
                int moveCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (moveCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = moveCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.ParentNode = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.ParentNode;
        }

        path.Reverse();

        _grid.path = path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (distX > distY)
        {
            return _diagonalWeight * distY + _horVerWeight * (distX - distY);
        }
        return _diagonalWeight * distX + _horVerWeight * (distY - distX);
    }
}
