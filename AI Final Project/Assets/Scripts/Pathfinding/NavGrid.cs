using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
    public LayerMask Unwalkable;
    public Vector2 GridWorldSize;
    public float NodeRadius;
    private Node[,] _grid;

    private float _nodeDiameter;
    private int _gridSizeX, _gridSizeY;

    public bool ShowGrid = true;
    public int MaxSize { get { return _gridSizeX * _gridSizeY; } }

    private void Start()
    {
        BakeGrid();
    }


    [ContextMenu("Bake NavGrid")]
    public void BakeGrid()
    {
        // Setting Grid Size from Parameters & Creating it.
        _nodeDiameter = NodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {

        _grid = new Node[_gridSizeX, _gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + NodeRadius) + Vector3.forward * (y * _nodeDiameter + NodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, NodeRadius, Unwalkable));

                // Populating the Grid Nodes
                _grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighboures = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // If Found Node Is Passed Node, Skip
                if (x == 0 && y == 0) { continue; }

                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                // Confirm Node Exist On Grid
                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    neighboures.Add(_grid[checkX, checkY]);
                }
            }
        }

        return neighboures;
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float percentageX = (worldPos.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float percentageY = (worldPos.z + GridWorldSize.y / 2) / GridWorldSize.y;
        percentageX = Mathf.Clamp01(percentageX);
        percentageY = Mathf.Clamp01(percentageY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentageX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentageY);
        return _grid[x, y];
    }


    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));

        if (_grid != null && ShowGrid)
        {
            foreach (Node node in _grid)
            {
                Color cWalkable = new Color(1, 1, 1, 0.5f);
                Color cUnwalkable = new Color(1, 0, 0, 0.5f);
                Gizmos.color = (node.Walkable) ? cWalkable : cUnwalkable;
                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.cyan;
                    }
                }
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - 0.1f));
            }
        }
    }
}
