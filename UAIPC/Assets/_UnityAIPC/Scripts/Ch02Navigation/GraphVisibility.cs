using UnityEngine;
using System.Collections.Generic;

public class GraphVisibility : Graph
{

    public override void Load()
    {
        Vertex[] verts = GameObject.FindObjectsOfType<Vertex>();
        vertices = new List<Vertex>(verts);
        for (int i = 0; i < vertices.Count; i++)
        {
            VertexVisibility vv = vertices[i] as VertexVisibility;
            vv.id = i;
            vv.FindNeighbours(vertices);
        }
    }

    public override Vertex GetNearestVertex(Vector3 position)
    {
        Vertex vertex = null;
        float dist = Mathf.Infinity;
        float distNear = dist;
        Vector3 posVertex = Vector3.zero;
        for (int i = 0; i < vertices.Count; i++)
        {
            posVertex = vertices[i].transform.position;
            dist = Vector3.Distance(position, posVertex);
            if (dist < distNear)
            {
                distNear = dist;
                vertex = vertices[i];
            }
        }
        return vertex;
    }

    public override Vertex[] GetNeighbours(Vertex v)
    {
        List<Edge> edges = v.neighbours;
        Vertex[] ns = new Vertex[edges.Count];
        int i;
        for (i = 0; i < edges.Count; i++)
        {
            ns[i] = edges[i].vertex;
        }
        return ns;
    }

    public override Edge[] GetEdges(Vertex v)
    {
        return vertices[v.id].neighbours.ToArray();
    }

}
