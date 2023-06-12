using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
    public Vector3 GridWorldSize;
    public float NodeRadius;
    public LayerMask Unwalkable;
    public TerrainType[] WalkbleRegions;
    LayerMask _walkableMask;
    Dictionary<int, int> _walkableRegionsDictionary = new Dictionary<int, int>();

    private Node[,,] _grid;

    private float _nodeDiameter;
    private int _gridSizeX, _gridSizeY, _gridSizeZ;

    private int _penaltyMin;
    private int _penaltyMax;

    public bool ShowGrid = false;
    public bool ShowPenalty = false;
    public bool ShowEmpty = false;

    public int MaxSize { get { return _gridSizeX * _gridSizeY * _gridSizeZ; } }


    private void Awake()
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
        _gridSizeZ = Mathf.RoundToInt(GridWorldSize.z / _nodeDiameter);


        int lowPen = 0;
        int highPen = 0;
        foreach (TerrainType region in WalkbleRegions)
        {
            _walkableMask.value |= region.TerrainMask.value;
            _walkableRegionsDictionary.Add((int)Mathf.Log(region.TerrainMask.value, 2), region.TerrainPenalty);

            // Setting MinMaxPenalty
            if (region.TerrainPenalty < lowPen)
            {
                lowPen = region.TerrainPenalty;
            }
            if (region.TerrainPenalty > highPen)
            {
                highPen = region.TerrainPenalty;
            }
        }
        _penaltyMin = lowPen;
        _penaltyMax = highPen;

        CreateGrid();
    }

    private void CreateGrid()
    {

        _grid = new Node[_gridSizeX, _gridSizeY, _gridSizeZ];
        Vector3 worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.up * GridWorldSize.y / 2 - Vector3.forward * GridWorldSize.z / 2;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                for (int z = 0; z < _gridSizeZ; z++)
                {

                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + NodeRadius) + Vector3.up * (y * _nodeDiameter + NodeRadius) + Vector3.forward * (z * _nodeDiameter + NodeRadius);
                    bool empty = !Physics.CheckSphere(worldPoint, NodeRadius, Unwalkable) && !Physics.CheckSphere(worldPoint, NodeRadius, _walkableMask);
                    bool walkable = !Physics.CheckSphere(worldPoint, NodeRadius, Unwalkable);

                    int movementPenalty = 0;
                    if (walkable)
                    {
                        Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 100, _walkableMask))
                        {
                            _walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                        }
                    }

                    // Populating the Grid Nodes
                    _grid[x, y, z] = new Node(empty, walkable, worldPoint, x, y, z, movementPenalty);
                }
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
                for (int z = -1; z <= 1; z++)
                {
                    // If Found Node Is Passed Node, Skip
                    if (x == 0 && y == 0 && z == 0) { continue; }

                    int checkX = node.GridX + x;
                    int checkY = node.GridY + y;
                    int checkZ = node.GridZ + z;

                    // Confirm Node Exist On Grid
                    if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY && checkZ >= 0 && checkZ < _gridSizeZ)
                    {
                        neighboures.Add(_grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }

        return neighboures;
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float percentageX = (worldPos.x - transform.position.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float percentageY = (worldPos.y - transform.position.y + GridWorldSize.y / 2) / GridWorldSize.y;
        float percentageZ = (worldPos.z - transform.position.z + GridWorldSize.z / 2) / GridWorldSize.z;
        percentageX = Mathf.Clamp01(percentageX);
        percentageY = Mathf.Clamp01(percentageY);
        percentageZ = Mathf.Clamp01(percentageZ);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentageX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentageY);
        int z = Mathf.RoundToInt((_gridSizeZ - 1) * percentageZ);
        return _grid[x, y, z];
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, GridWorldSize.y, GridWorldSize.z));

        if (_grid != null && ShowGrid)
        {
            foreach (Node node in _grid)
            {
                Color cEmpty = new Color(0, 0.1f, 0, 0.1f);
                Color cWalkable = new Color(1, 1, 1, 0.75f);
                Color cPenalty = new Color(0, 0, 0, 0.75f);
                Color cUnwalkable = new Color(1, 0, 0, 0.75f);
                if (!node.Empty)
                {
                    if (!ShowPenalty)
                    {
                        Gizmos.color = (node.Walkable) ? cWalkable : cUnwalkable;
                    }
                    else
                    {
                        Gizmos.color = Color.Lerp(cWalkable, cPenalty, Mathf.InverseLerp(_penaltyMin, _penaltyMax, node.MovementPenalty));
                        Gizmos.color = (node.Walkable) ? Gizmos.color : cUnwalkable;
                    }

                    Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - 0.1f));
                }
                else if (node.Empty && ShowEmpty)
                {
                    Gizmos.color = cEmpty;
                    Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - 0.1f));
                }
            }
        }
    }


    [System.Serializable]
    public class TerrainType
    {
        public LayerMask TerrainMask;
        public int TerrainPenalty;
    }
}
