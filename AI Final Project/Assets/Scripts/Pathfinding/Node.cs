using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool Empty;
    public bool Walkable;
    public Vector3 WorldPosition;
    public int GridX;
    public int GridY;
    public int GridZ;
    public int MovementPenalty;

    public Node ParentNode;
    public int GCost;
    public int HCost;
    int _heapIndex;

    public int FCost { get { return GCost + HCost; } }
    public int HeapIndex { get => _heapIndex; set => _heapIndex = value; }

    public Node(bool empty, bool walkable, Vector3 worldPosition, int gridX, int gridY, int gridZ, int movementPenalty)
    {
        Empty = empty;
        Walkable = walkable;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
        GridZ = gridZ;
        MovementPenalty = movementPenalty;
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(nodeToCompare.HCost);
        }
        return -compare;
    }
}
