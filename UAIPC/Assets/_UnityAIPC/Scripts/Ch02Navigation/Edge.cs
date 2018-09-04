/*
 * File: Edge.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Created Date: 2018-03-18 13:24:49
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-02-28 21:07:30
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using System;
/// <summary>
/// Edge is an object that holds a vertex and its cost
/// </summary>
[System.Serializable]
public class Edge : IComparable<Edge>
{
    public float cost;
    public Vertex vertex;

    public Edge(Vertex vertex = null, float cost = 1f)
    {
        this.vertex = vertex;
        this.cost = cost;
    }

    public int CompareTo(Edge other)
    {
        float result = cost - other.cost;
        int idA = vertex.GetInstanceID();
        int idB = other.vertex.GetInstanceID();
        if (idA == idB)
            return 0;
        return (int)result;
    }

    public bool Equals(Edge other)
    {
        return (other.vertex.id == this.vertex.id);
    }

    public override bool Equals(object obj)
    {
        Edge other = (Edge)obj;
        return (other.vertex.id == this.vertex.id);
    }

    public override int GetHashCode()
    {
        return this.vertex.GetHashCode();
    }
}