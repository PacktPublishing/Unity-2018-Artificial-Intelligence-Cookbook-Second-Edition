/*
 * File: Vertex.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Created Date: 2018-03-17 22:04:23
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-02-28 21:07:30
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Vertex : MonoBehaviour
{
    /// <summary>
    /// Vertex ID
    /// </summary>
    public int id;
    /// <summary>
    /// Vertex neighbours
    /// </summary>
    public List<Edge> neighbours;
    [HideInInspector]
    public Vertex prev;
}
