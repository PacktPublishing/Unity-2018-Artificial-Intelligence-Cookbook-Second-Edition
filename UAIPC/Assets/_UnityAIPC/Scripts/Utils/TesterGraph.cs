using UnityEngine;
using System.Collections.Generic;

public enum TesterGraphAlgorithm
{
    BFS, DFS, DIJKSTRA, ASTAR
}

public class TesterGraph : MonoBehaviour
{
    public Graph graph;
    public TesterGraphAlgorithm algorithm;
    public bool smoothPath;
    public string vertexTag = "Vertex";
    public string obstacleTag = "Wall";
    public Color pathColor;
    [Range(0.1f, 1f)]
    public float pathNodeRadius = .3f;
    Camera mainCamera;
    GameObject srcObj;
    GameObject dstObj;
    List<Vertex> path;

    void Awake()
    {
        mainCamera = Camera.main;
        srcObj = null;
        dstObj = null;
        path = new List<Vertex>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            srcObj = GetNodeFromScreen(Input.mousePosition);
        }
        dstObj = GetNodeFromScreen(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (path.Count != 0)
            {
                ShowPath(path, Color.white);
                path = new List<Vertex>();
            }
            switch (algorithm)
            {
                case TesterGraphAlgorithm.ASTAR:
                    path = graph.GetPathAstar(srcObj, dstObj, null);
                    break;
                default:
                case TesterGraphAlgorithm.BFS:
                    path = graph.GetPathBFS(srcObj, dstObj);
                    break;
                case TesterGraphAlgorithm.DFS:
                    path = graph.GetPathDFS(srcObj, dstObj);
                    break;
                case TesterGraphAlgorithm.DIJKSTRA:
                    path = graph.GetPathDijkstra(srcObj, dstObj);
                    break;
                //case TesterGraphAlgorithm.IDASTAR:
                //    path = graph.GetPathIDAstar(srcObj, dstObj);
                //    break;
            }
            if (smoothPath)
                path = graph.Smooth(path);
            //ShowPath(path, pathColor);
        }
    }

    public void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (ReferenceEquals(graph, null))
            return;
        Vertex v;
        if (!ReferenceEquals(srcObj, null))
        {
            Gizmos.color = Color.green;
            v = graph.GetNearestVertex(srcObj.transform.position);
            Gizmos.DrawSphere(v.transform.position, pathNodeRadius);
        }
        if (!ReferenceEquals(dstObj, null))
        {
            Gizmos.color = Color.red;
            v = graph.GetNearestVertex(dstObj.transform.position);
            Gizmos.DrawSphere(v.transform.position, pathNodeRadius);
        }
        int i;
        Gizmos.color = pathColor;
        for (i = 0; i < path.Count; i++)
        {
            v = path[i];
            Gizmos.DrawSphere(v.transform.position, pathNodeRadius);
            if (smoothPath && i != 0)
            {
                Vertex prev = path[i - 1];
                Gizmos.DrawLine(v.transform.position, prev.transform.position);
                
            }

        }
    }

    public void ShowPath(List<Vertex> path, Color color)
    {
        int i;
        for (i = 0; i < path. Count; i++)
        {
            Vertex v = path[i];
            Renderer r = v.GetComponent<Renderer>();
            if (ReferenceEquals(r, null))
                continue;
            r.material.color = color;
        }
    }

    


    private GameObject GetNodeFromScreen(Vector3 screenPosition)
    {
        GameObject node = null;
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (RaycastHit h in hits)
        {
            if (!h.collider.CompareTag(vertexTag) && !h.collider.CompareTag(obstacleTag))
                continue;
            node = h.collider.gameObject;
            break;
        }
        return node;
    }
}
