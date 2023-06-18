using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathResult> _resultsQueue = new Queue<PathResult>();

    static PathRequestManager instance;
    Pathfinding _pathfinding;


    private void Awake()
    {
        instance = this;
        _pathfinding = GetComponent<Pathfinding>();
    }

    private void Update()
    {
        if (_resultsQueue.Count > 0)
        {
            int itemsInQueue = _resultsQueue.Count;
            lock (_resultsQueue)
            {
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = _resultsQueue.Dequeue();
                    result.Callback(result.path, result.success);
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance._pathfinding.FindPath(request, instance.FinishedProcessingPath);
        };

        threadStart.Invoke();
    }

    public void FinishedProcessingPath(PathResult result)
    {
        lock (_resultsQueue)
        {
            _resultsQueue.Enqueue(result);
        }
    }
}


public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> Callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        Callback = callback;
    }
}


public struct PathRequest
{
    public Vector3 PathStart;
    public Vector3 PathEnd;
    public bool CountAir;
    public Action<Vector3[], bool> Callback;

    public PathRequest(Vector3 start, Vector3 end, bool canFly, Action<Vector3[], bool> callback)
    {
        PathStart = start;
        PathEnd = end;
        CountAir = canFly;
        Callback = callback;
    }
}
