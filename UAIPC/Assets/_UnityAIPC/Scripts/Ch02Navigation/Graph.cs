/*
 * File: Graph.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Created Date: 2018-03-17 21:51:41
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-02-28 21:07:30
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract class for graphs
/// </summary>
public abstract class Graph : MonoBehaviour
{

    public GameObject vertexPrefab;
    protected List<Vertex> vertices;
    protected List<List<Vertex>> neighbors;
    protected List<List<float>> costs;
    protected Dictionary<int, int> instIdToId;

    //// this is for informed search like A*
    public delegate float Heuristic(Vertex a, Vertex b);

    // Used for getting path in frames
    public List<Vertex> path;
    public bool isFinished;
    
    public virtual void Start()
    {
        Load();
    }

    public virtual void Load() { }

    public virtual int GetSize()
    {
        if (ReferenceEquals(vertices, null))
            return 0;
        return vertices.Count;
    }

    public virtual Vertex GetNearestVertex(Vector3 position)
    {
        return null;
    }


    public virtual Vertex[] GetNeighbours(Vertex v)
    {
        if (ReferenceEquals(neighbors, null) || neighbors.Count == 0)
            return new Vertex[0];
        if (v.id < 0 || v.id >= neighbors.Count)
            return new Vertex[0];
        return neighbors[v.id].ToArray();
    }

    public virtual Edge[] GetEdges(Vertex v)
    {
        if (ReferenceEquals(neighbors, null) || neighbors.Count == 0)
            return new Edge[0];
        if (v.id < 0 || v.id >= neighbors.Count)
            return new Edge[0];
        int numEdges = neighbors[v.id].Count;
        Edge[] edges = new Edge[numEdges];
        List<Vertex> vertexList = neighbors[v.id];
        List<float> costList = costs[v.id];
        for (int i = 0; i < numEdges; i++)
        {
            edges[i] = new Edge();
            edges[i].cost = costList[i];
            edges[i].vertex = vertexList[i];
        }
        return edges;
    }

    public List<Vertex> GetPathBFS(GameObject srcObj, GameObject dstObj)
    {
        if (srcObj == null || dstObj == null)
            return new List<Vertex>();
        Vertex[] neighbours;
        Queue<Vertex> q = new Queue<Vertex>();
        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        Vertex v;
        int[] previous = new int[vertices.Count];
        for (int i = 0; i < previous.Length; i++)
            previous[i] = -1;
        previous[src.id] = src.id;
        q.Enqueue(src);
        while (q.Count != 0)
        {
            v = q.Dequeue();
            if (ReferenceEquals(v, dst))
            {
                return BuildPath(src.id, v.id, ref previous);
            }

            neighbours = GetNeighbours(v);
            foreach (Vertex n in neighbours)
            {
                if (previous[n.id] != -1)
                    continue;
                previous[n.id] = v.id;
                q.Enqueue(n);
            }
        }
        return new List<Vertex>();
    }

    public List<Vertex> GetPathDFS(GameObject srcObj, GameObject dstObj)
    {
        if (srcObj == null || dstObj == null)
            return new List<Vertex>();
        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        Vertex[] neighbours;
        Vertex v;
        int[] previous = new int[vertices.Count];
        for (int i = 0; i < previous.Length; i++)
            previous[i] = -1;
        previous[src.id] = src.id;
        Stack<Vertex> s = new Stack<Vertex>();
        s.Push(src);
        while (s.Count != 0)
        {
            v = s.Pop();
            if (ReferenceEquals(v, dst))
            {
                return BuildPath(src.id, v.id, ref previous);
            }

            neighbours = GetNeighbours(v);
            foreach (Vertex n in neighbours)
            {
                if (previous[n.id] != -1)
                    continue;
                previous[n.id] = v.id;
                s.Push(n);
            }
        }
        return new List<Vertex>();
    }

    public List<Vertex> GetPathDijkstra(GameObject srcObj, GameObject dstObj)
    {
        if (srcObj == null || dstObj == null)
            return new List<Vertex>();
        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        GPWiki.BinaryHeap<Edge> frontier = new GPWiki.BinaryHeap<Edge>();
        Edge[] edges;
        Edge node, child;
        int size = vertices.Count;
        float[] distValue = new float[size];
        int[] previous = new int[size];
        node = new Edge(src, 0);
        frontier.Add(node);
        distValue[src.id] = 0;
        previous[src.id] = src.id; 
        for (int i = 0; i < size; i++)
        {
            if (i == src.id)
                continue;
            distValue[i] = Mathf.Infinity;
            previous[i] = -1;
        }
        while (frontier.Count != 0)
        {
            node = frontier.Remove();
            int nodeId = node.vertex.id;
            // exit if necessary
            if (ReferenceEquals(node.vertex, dst))
            {
                return BuildPath(src.id, node.vertex.id, ref previous);
            }
            edges = GetEdges(node.vertex);
            foreach (Edge e in edges)
            {
                int eId = e.vertex.id;
                if (previous[eId] != -1)
                    continue;
                float cost = distValue[nodeId] + e.cost;
                if (cost < distValue[e.vertex.id])
                {
                    distValue[eId] = cost;
                    previous[eId] = nodeId;
                    frontier.Remove(e);
                    child = new Edge(e.vertex, cost);
                    frontier.Add(child);
                }
            }
        }
        return new List<Vertex>();
    }

    public List<Vertex> GetPathAstar(GameObject srcObj, GameObject dstObj, Heuristic h = null)
    {
        if (srcObj == null || dstObj == null)
            return new List<Vertex>();
        if (ReferenceEquals(h, null))
            h = EuclidDist;

        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        GPWiki.BinaryHeap<Edge> frontier = new GPWiki.BinaryHeap<Edge>();
        Edge[] edges;
        Edge node, child;
        int size = vertices.Count;
        float[] distValue = new float[size];
        int[] previous = new int[size];
        node = new Edge(src, 0);
        frontier.Add(node);
        distValue[src.id] = 0;
        previous[src.id] = src.id;
        for (int i = 0; i < size; i++)
        {
            if (i == src.id)
                continue;
            distValue[i] = Mathf.Infinity;
            previous[i] = -1;
        }
        while (frontier.Count != 0)
        {
            node = frontier.Remove();
            int nodeId = node.vertex.id;
            if (ReferenceEquals(node.vertex, dst))
            {                 
                return BuildPath(src.id, node.vertex.id, ref previous);
            }
            edges = GetEdges(node.vertex);
            foreach (Edge e in edges)
            {
                int eId = e.vertex.id;
                if (previous[eId] != -1)
                    continue;
                float cost = distValue[nodeId] + e.cost;
                // key point
                cost += h(node.vertex, e.vertex);
                if (cost < distValue[e.vertex.id])
                {
                    distValue[eId] = cost;
                    previous[eId] = nodeId;
                    frontier.Remove(e);
                    child = new Edge(e.vertex, cost);
                    frontier.Add(child);
                }
            }
        }
        return new List<Vertex>();
    }


    public List<Vertex> GetPathIDAstar(GameObject srcObj, GameObject dstObj, Heuristic h = null)
    {
        if (srcObj == null || dstObj == null)
            return new List<Vertex>();
        if (ReferenceEquals(h, null))
            h = EuclidDist;

        List<Vertex> path = new List<Vertex>();
        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        Vertex goal = null;
        bool[] visited = new bool[vertices.Count];
        for (int i = 0; i < visited.Length; i++)
            visited[i] = false;
        visited[src.id] = true;
        float bound = h(src, dst);
        while (bound < Mathf.Infinity)
        {
            bound = RecursiveIDAstar(src, dst, bound, h, ref goal, ref visited);
        }
        if (ReferenceEquals(goal, null))
            return path;
        return BuildPath(goal);
    }

    private float RecursiveIDAstar(
            Vertex v,
            Vertex dst,
            float bound,
            Heuristic h,
            ref Vertex goal,
            ref bool[] visited)
    {
        if (ReferenceEquals(v, dst))
            return Mathf.Infinity;
        Edge[] edges = GetEdges(v);
        if (edges.Length == 0)
            return Mathf.Infinity;
        float fn = Mathf.Infinity;
        foreach (Edge e in edges)
        {
            int eId = e.vertex.id;
            if (visited[eId])
                continue;
            visited[eId] = true;
            e.vertex.prev = v;
            float f = h(v, dst);
            float b;
            if (f <= bound)
            {
                b = RecursiveIDAstar(e.vertex, dst, bound, h, ref goal, ref visited);
                fn = Mathf.Min(f, b);
            }
            else
                fn = Mathf.Min(fn, f);
        }
        return fn;
    }

    public IEnumerator GetPathInFrames(GameObject srcObj, GameObject dstObj, Heuristic h = null)
    {
        // changes over A*
        isFinished = false;
        path = new List<Vertex>();
        if (srcObj == null || dstObj == null)
        {
            path = new List<Vertex>();
            isFinished = true;
            yield break;
        }
        //////////////////////////////
        
        if (ReferenceEquals(h, null))
            h = EuclidDist;

        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        GPWiki.BinaryHeap<Edge> frontier = new GPWiki.BinaryHeap<Edge>();
        
        Edge[] edges;
        Edge node, child;
        int size = vertices.Count;
        float[] distValue = new float[size];
        int[] previous = new int[size];
        node = new Edge(src, 0);
        frontier.Add(node);
        distValue[src.id] = 0;
        previous[src.id] = src.id;
        for (int i = 0; i < size; i++)
        {
            if (i == src.id)
                continue;
            distValue[i] = Mathf.Infinity;
            previous[i] = -1;
        }
        while (frontier.Count != 0)
        {
            // changes over A*
            yield return null;
            //////////////////////////////
            node = frontier.Remove();
            int nodeId = node.vertex.id;
            if (ReferenceEquals(node.vertex, dst))
            {
                // changes over A*
                path = BuildPath(src.id, node.vertex.id, ref previous);
                break;
                //////////////////////////////
            }
            edges = GetEdges(node.vertex);
            foreach (Edge e in edges)
            {
                int eId = e.vertex.id;
                if (previous[eId] != -1)
                    continue;
                float cost = distValue[nodeId] + e.cost;
                // key point
                cost += h(node.vertex, e.vertex);
                if (cost < distValue[e.vertex.id])
                {
                    distValue[eId] = cost;
                    previous[eId] = nodeId;
                    frontier.Remove(e);
                    child = new Edge(e.vertex, cost);
                    frontier.Add(child);
                }
            }
        }
        // chages over A*
        isFinished = true;
        yield break;
        //////////////////////////////
    }


    public List<Vertex> Smooth(List<Vertex> path)
    {
        List<Vertex> newPath = new List<Vertex>();
        if (path.Count == 0)
            return newPath;
        if (path.Count < 3)
            return path;
        newPath.Add(path[0]);
        int i, j;
        for (i = 0; i < path.Count - 1;)
        {
            for (j = i + 1; j < path.Count; j++)
            {
                Vector3 origin = path[i].transform.position;
                Vector3 destination = path[j].transform.position;
                Vector3 direction = destination - origin;
                float distance = direction.magnitude;
                bool isWall = false;
                direction.Normalize();
                Ray ray = new Ray(origin, direction);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray, distance);
                foreach (RaycastHit hit in hits)
                {
                    string tag = hit.collider.gameObject.tag;
                    if (tag.Equals("Wall"))
                    {
                        isWall = true;
                        break;
                    }
                }
                if (isWall)
                    break;
            }
            i = j - 1;
            newPath.Add(path[i]);
        }
        return newPath;
    }



    public void SetPathAmbush(GameObject dstObj, List<Lurker> lurkers)
    {
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        foreach (Lurker l in lurkers)
        {
            Vertex src = GetNearestVertex(l.transform.position);
            l.path = AStarMbush(src, dst, l, lurkers);
        }

    }

    public List<Vertex> AStarMbush(
            Vertex src,
            Vertex dst,
            Lurker agent,
            List<Lurker> lurkers,
            Heuristic h = null)
    {
        if (ReferenceEquals(h, null))
            h = EuclidDist;

        int graphSize = vertices.Count;
        float[] extra = new float[graphSize];
        float[] costs = new float[graphSize];
        int i;
        for (i = 0; i < graphSize; i++)
        {
            extra[i] = 1f;
            costs[i] = Mathf.Infinity;
        }

        foreach (Lurker l in lurkers)
        {
            foreach (Vertex v in l.path)
            {
                extra[v.id] += 1f;
            }
        }
        Edge[] successors;
        
        int[] previous = new int[graphSize];
        for (i = 0; i < graphSize; i++)
            previous[i] = -1;
        previous[src.id] = src.id;
        float cost = 0;
        Edge node = new Edge(src, 0);
        
        GPWiki.BinaryHeap<Edge> frontier = new GPWiki.BinaryHeap<Edge>();
        frontier.Add(node);
        while (frontier.Count != 0)
        {        
            node = frontier.Remove();
            if (ReferenceEquals(node.vertex, dst))
                return BuildPath(src.id, node.vertex.id, ref previous);
            int nodeId = node.vertex.id;
            if (node.cost > costs[nodeId])
                continue;

            successors = GetEdges(node.vertex);
            foreach (Edge e in successors)
            {
                int eId = e.vertex.id;
                if (previous[eId] != -1)
                    continue;

                cost = e.cost;
                cost += costs[dst.id];
                cost += h(e.vertex, dst);
                if (cost < costs[e.vertex.id])
                {
                    Edge child;
                    child = new Edge(e.vertex, cost);
                    costs[eId] = cost;
                    previous[eId] = nodeId;
                    frontier.Remove(e);
                    frontier.Add(child);
                }
            }
        }
        return new List<Vertex>();
    }

    private List<Vertex> BuildPath(int srcId, int dstId, ref int[] prevList)
    {
        List<Vertex> path = new List<Vertex>();
        int prev = dstId;
        do
        {
            path.Add(vertices[prev]);
            prev = prevList[prev];
        } while (prev != srcId);
        return path;
    }

    // for IDA*
    private List<Vertex> BuildPath(Vertex v)
    {
        List<Vertex> path = new List<Vertex>();
        while (!ReferenceEquals(v, null))
        {
            path.Add(v);
            v = v.prev;
        }
        return path;
    }

    public float EuclidDist(Vertex a, Vertex b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        return  Vector3.Distance(posA, posB);
    }

    public float ManhattanDist(Vertex a, Vertex b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
    }
}
