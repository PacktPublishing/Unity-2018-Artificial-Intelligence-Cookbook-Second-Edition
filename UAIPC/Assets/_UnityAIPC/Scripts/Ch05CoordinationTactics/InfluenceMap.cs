using UnityEngine;
using System.Collections.Generic;

public class InfluenceMap : Graph
{
    public List<Unit> unitList;
    // works as vertices in regular graph
    GameObject[] locations;

    // map flooding
    public float dropOffThreshold;
    // map flooding (components added to the game object)
    private Guild[] guildList;

    void Awake()
    {
        if (unitList == null)
            unitList = new List<Unit>();

        // map flooding
        guildList = gameObject.GetComponents<Guild>();
    }

    public void AddUnit(Unit u)
    {
        if (unitList.Contains(u))
            return;
        unitList.Add(u);
    }

    public void RemoveUnit(Unit u)
    {
        unitList.Remove(u);
    }

    public void ComputeInfluenceSimple()
    {
        VertexInfluence v;
        float dropOff;
        List<Vertex> pending = new List<Vertex>();
        List<Vertex> visited = new List<Vertex>();
        List<Vertex> frontier;
        Vertex[] neighbours;

        foreach (Unit u in unitList)
        {
            Vector3 uPos = u.transform.position;
            Vertex vert = GetNearestVertex(uPos);
            pending.Add(vert);
            // BFS for assigning influence
            for (int i = 1; i <= u.radius; i++)
            {
                frontier = new List<Vertex>();
                foreach (Vertex p in pending)
                {
                    if (visited.Contains(p))
                        continue;
                    visited.Add(p);
                    v = p as VertexInfluence;
                    dropOff = u.GetDropOff(i);
                    v.SetValue(u.faction, dropOff);
                    neighbours = GetNeighbours(vert);
                    frontier.AddRange(neighbours);
                }
                pending = new List<Vertex>(frontier);
            }
        }
    }

    public List<GuildRecord> ComputeMapFlooding()
    {
        GPWiki.BinaryHeap<GuildRecord> open;
        open = new GPWiki.BinaryHeap<GuildRecord>();
        List<GuildRecord> closed;
        closed = new List<GuildRecord>();
        foreach (Guild g in guildList)
        {
            GuildRecord gr = new GuildRecord();
            Vector3 pos = g.baseObject.transform.position;
            gr.location = GetNearestVertex(pos);
            gr.guild = g;
            gr.strength = g.GetDropOff(0f);
            open.Add(gr);
        }
        while (open.Count != 0)
        {
            GuildRecord current;
            current = open.Remove();
            Vertex v = current.location;
            Vector3 currPos;
            currPos = v.transform.position;
            Vertex[] neighbours;
            neighbours = GetNeighbours(v);
            foreach (Vertex n in neighbours)
            {
                Vector3 nPos = n.transform.position;
                float dist = Vector3.Distance(currPos, nPos);
                float strength = current.guild.GetDropOff(dist);
                if (strength < dropOffThreshold)
                    continue;
                GuildRecord neighGR = new GuildRecord();
                neighGR.location = n;
                neighGR.strength = strength;
                VertexInfluence vi;
                vi = n as VertexInfluence;
                neighGR.guild = vi.guild;
                if (closed.Contains(neighGR))
                {
                    Vertex location = neighGR.location;
                    int index = closed.FindIndex(x => x.location == location);
                    GuildRecord gr = closed[index];
                    if (gr.guild.name != current.guild.name
                            && gr.strength < strength)
                        continue;
                }
                else if (open.Contains(neighGR))
                {
                    bool mustContinue = false;
                    foreach (GuildRecord gr in open)
                    {
                        if (gr.Equals(neighGR))
                        {
                            mustContinue = true;
                            break;
                        }
                    }
                    if (mustContinue)
                        continue;
                }
                else
                {
                    neighGR = new GuildRecord();
                    neighGR.location = n;
                }
                neighGR.guild = current.guild;
                neighGR.strength = strength;
                open.Add(neighGR);
            }
            closed.Add(current);
        }
        return closed;
    }

    public static void Convolve(
            float[,] matrix,
            ref float[,] source,
            ref float[,] destination)
    {
        int matrixLength = matrix.GetLength(0);
        int size = (int)(matrixLength - 1) / 2;
        int height = source.GetLength(0);
        int width = source.GetLength(1);
        int i, j, k, m;
        for (i = 0; i < width - size; i++)
        {
            for (j = 0; j < height - size; j++)
            {
                destination[i, j] = 0f;
                for (k = 0; k < matrixLength; k++)
                {
                    for (m = 0; m < matrixLength; m++)
                    {
                        int row = i + k - size;
                        int col = j + m - size;
                        float aux = source[row, col] * matrix[k, m];
                        destination[i, j] += aux;
                    }
                }
            }
        }
    }

    public static void ConvolveDriver(
            float[,] matrix,
            ref float[,] source,
            ref float[,] destination,
            int iterations)
    {
        float[,] map1;
        float[,] map2;
        if (iterations % 2 == 0)
        {
            map1 = source;
            map2 = destination;
        }
        else
        {
            destination = source;
            map1 = destination;
            map2 = source;
        }
        int i;
        for (i = 0; i < iterations; i++)
        {
            Convolve(matrix, ref source, ref destination);
            float[,] aux = map1;
            map1 = map2;
            map2 = aux;
        }
    }
}