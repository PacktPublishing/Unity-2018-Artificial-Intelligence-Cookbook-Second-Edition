/*
* File: DungeonNode2D.cs
* Project: Unity 2018 AI Programming Cookbook
* Author: Jorge Palacios
* -----
* Last Modified: 2018-06-10 22:54:33
* Modified By: Jorge Palacios
* -----
* Copyright (c) 2018 Packt Publishing Ltd
*/

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DungeonNode2D
{
  public Rect area;
  public Rect block;
  public Dungeon2D root;
  public DungeonNode2D left;
  public DungeonNode2D right;
  protected int depth;

  public DungeonNode2D (Rect area, Dungeon2D root, int depth = 0)
  {
    this.area = area;
    this.root = root;
    this.depth = depth;
    this.root.leaves.Add(this);
    if (!this.root.tree.ContainsKey(depth))
      this.root.tree.Add(depth, new List<DungeonNode2D>());
    this.root.tree[depth].Add(this);
  }

  public void Split(Dungeon2D.Split splitCall)
  {
    this.root.leaves.Remove(this);
    Rect[] areas = splitCall(area);
    if (areas == null)
      return;
    left = new DungeonNode2D(areas[0], root, depth + 1);
    right = new DungeonNode2D(areas[1], root, depth + 1);
  }

  public void CreateBlock()
  {
    block = new Rect();
    block.xMin = Random.Range(area.xMin, area.center.x);
    block.yMin = Random.Range(area.yMin, area.center.y);
    block.xMax = Random.Range(area.center.x, area.xMax);
    block.yMax = Random.Range(area.center.y, area.yMax);
  }
  
}
