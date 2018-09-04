/*
 * File: GraphGrid.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Created Date: 2018-03-18 15:53:17
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-02-28 21:07:30
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class GraphGrid : Graph
{
    public GameObject obstaclePrefab;
    public string mapName = "arena.map";
    public bool get8Vicinity = false;
    public float cellSize = 1f;
    [Range(0, Mathf.Infinity)]
    public float defaultCost = 1f;
    [Range(0, Mathf.Infinity)]
    public float maximumCost = Mathf.Infinity;
    string mapsDir = "Maps";
    int numCols;
    int numRows;
    GameObject[] vertexObjs;
    bool[,] mapVertices;

    private int GridToId(int x, int y)
    {
        return Math.Max(numRows, numCols) * y + x;
    }

    private Vector2 IdToGrid(int id)
    {
        Vector2 location = Vector2.zero;
        location.y = Mathf.Floor(id / numCols);
        location.x = Mathf.Floor(id % numCols);
        return location;
    }

    private void LoadMap(string filename)
    {
        string path = Application.dataPath + "/" + mapsDir + "/" + filename;
        try
        {
            StreamReader strmRdr = new StreamReader(path);
            using (strmRdr)
            {
                int j = 0;
                int i = 0;
                int id = 0;
                string line;

                Vector3 position = Vector3.zero;
                Vector3 scale = Vector3.zero;
                line = strmRdr.ReadLine();// non-important line
                line = strmRdr.ReadLine();// height
                numRows = int.Parse(line.Split(' ')[1]);
                line = strmRdr.ReadLine();// width
                numCols = int.Parse(line.Split(' ')[1]);
                line = strmRdr.ReadLine();// "map" line in file
                
                vertices = new List<Vertex>(numRows * numCols);
                neighbors = new List<List<Vertex>>(numRows * numCols);
                costs = new List<List<float>>(numRows * numCols);
                vertexObjs = new GameObject[numRows * numCols];
                mapVertices = new bool[numRows, numCols];

                for (i = 0; i < numRows; i++)
                {
                    line = strmRdr.ReadLine();
                    for (j = 0; j < numCols; j++)
                    {
                        bool isGround = true;
                        if (line[j] != '.')
                            isGround = false;
                        mapVertices[i, j] = isGround;
                        position.x = j * cellSize;
                        position.z = i * cellSize;
                        id = GridToId(j, i);
                        if (isGround)
                            vertexObjs[id] = Instantiate(vertexPrefab, position, Quaternion.identity) as GameObject;
                        else
                            vertexObjs[id] = Instantiate(obstaclePrefab, position, Quaternion.identity) as GameObject;
                        vertexObjs[id].name = vertexObjs[id].name.Replace("(Clone)", id.ToString());
                        Vertex v = vertexObjs[id].AddComponent<Vertex>();
                        v.id = id;
                        vertices.Add(v);
                        neighbors.Add(new List<Vertex>());
                        costs.Add(new List<float>());
                        float y = vertexObjs[id].transform.localScale.y;
                        scale = new Vector3(cellSize, y, cellSize);
                        vertexObjs[id].transform.localScale = scale;
                        vertexObjs[id].transform.parent = gameObject.transform;
                    }
                }

                // now onto the neighbours
                for (i = 0; i < numRows; i++)
                {
                    for (j = 0; j < numCols; j++)
                    {
                        SetNeighbours(j, i);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    protected void SetNeighbours(int x, int y, bool get8 = false)
    {
        int col = x;
        int row = y;
        int i, j;
        int vertexId = GridToId(x, y);
        neighbors[vertexId] = new List<Vertex>();
        costs[vertexId] = new List<float>();
        Vector2[] pos = new Vector2[0];
        if (get8)
        {
            pos = new Vector2[8];
            int c = 0;
            for (i = row - 1; i <= row + 1; i++)
            {
                for (j = col -1; j <= col; j++)
                {
                    pos[c] = new Vector2(j, i);
                    c++;
                }
            }       
        }
        else
        {
            pos = new Vector2[4];
            pos[0] = new Vector2(col, row - 1);
            pos[1] = new Vector2(col - 1, row);
            pos[2] = new Vector2(col + 1, row);
            pos[3] = new Vector2(col, row + 1);   
        }
        foreach (Vector2 p in pos)
        {
            i = (int)p.y;
            j = (int)p.x;
            if (i < 0 || j < 0)
                continue;
            if (i >= numRows || j >= numCols)
                continue;
            if (i == row && j == col)
                continue;
            if (!mapVertices[i, j])
                continue;
            int id = GridToId(j, i);
            neighbors[vertexId].Add(vertices[id]);
            costs[vertexId].Add(defaultCost);
        }
    }

    public override void Load()
    {
        LoadMap(mapName);
    }

    public override Vertex GetNearestVertex(Vector3 position)
    {
        int col = (int)(position.x / cellSize);
        int row = (int)(position.z / cellSize);
        Vector2 p = new Vector2(col, row);
        List<Vector2> explored = new List<Vector2>();
        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(p);
        do
        {
            p = queue.Dequeue();
            col = (int)p.x;
            row = (int)p.y;
            int id = GridToId(col, row);
            if (mapVertices[row, col])
                return vertices[id];            
                
            if (!explored.Contains(p))
            {
                explored.Add(p);
                int i, j;
                for (i = row - 1; i <= row + 1; i++)
                {
                    for (j = col - 1; j <= col + 1; j++)
                    { 
                        if (i < 0 || j < 0)
                            continue;
                        if (j >= numCols || i >= numRows)
                            continue;
                        if (i == row && j == col)
                            continue;
                        queue.Enqueue(new Vector2(j, i));
                    }
                }
            }
        } while (queue.Count != 0);
        return null;
    }

}
