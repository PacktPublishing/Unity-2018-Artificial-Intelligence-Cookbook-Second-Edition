/*
 * File: DFSDungeon.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-18 10:13:22
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System.Collections.Generic;

public class DFSDungeon : MonoBehaviour
{
  public int width;
  public int height;
  public bool[,] dungeon;
  public bool[,] visited;
  private Stack<Vector2> stack;
  private Vector2 current;
  private int size;

  private void Init()
  {
    stack = new Stack<Vector2>();
    size = width * height;
    dungeon = new bool[height, width];
    visited = new bool[height, width];
    current.x = Random.Range(0, width - 1);
    current.y = Random.Range(0, height - 1);
    int i, j;
    for (j = 0; j < height; j++)
      for (i = 0; i < width; i++)
        dungeon[j, i] = true;
    stack.Push(current);
    i = (int)current.x;
    j = (int)current.y;
    visited[j, i] = true;
    size--;
  }

  public void Build()
  {
    Init();
    while (size > 0)
    {
      Vector2[] neighbors = GetNeighbors(current);
      if (neighbors.Length > 0)
      {
        int rand = Random.Range(0, neighbors.Length - 1);
        Vector2 n = neighbors[rand];
        stack.Push(current);
        int i, j;
        i = (int)current.y;
        j = (int)current.x;
        dungeon[j, i] = false;
        i = (int)n.y;
        j = (int)n.x;
        dungeon[j, i] = false;
        visited[j, i] = true;
        current = n;
        size--;
      }
      else if (stack.Count > 0)
        current = stack.Pop();
    }
  }

  /// <summary>
  /// Retrieves 8 vicinity
  /// </summary>
  /// <param name="node"></param>
  /// <returns></returns>
  private Vector2[] GetNeighbors(Vector2 node)
  {
    List<Vector2> neighbors = new List<Vector2>();
    int originX, targetX, originY, targetY;
    originX = (int)node.x - 1;
    originY = (int)node.y - 1;
    targetX = (int)node.x + 1;
    targetY = (int)node.y + 1;
    int i, j;
    for (j = originY; j < targetY; j++)
    {
      if (j < 0 || j >= height)
        continue;
      for (i = originX; i < targetX; i++)
      {
        if (i < 0 || i >= width)
        if (i == node.x && j == node.y)
          continue;
        if (visited[j,i])
          continue;
        neighbors.Add(new Vector2(i, j));
      }
    }
    return neighbors.ToArray();
  }
}