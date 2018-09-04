/*
 * File: Dungeon2D.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-19 00:45:21
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System.Collections.Generic;

public class Dungeon2D : MonoBehaviour
{
  /// <summary>
  /// Minimum Acceptable Size
  /// </summary>
  public float minAcceptSize;
  /// <summary>
  /// Dungeon area
  /// </summary>
  public Rect area;
  public Dictionary<int, List<DungeonNode2D>> tree;
  public HashSet<DungeonNode2D> leaves;

  public delegate Rect[] Split(Rect area);
  public Split splitCall;

  public DungeonNode2D root;
  
  public void Init()
  {
    leaves.Clear();
    tree.Clear();
    if (splitCall == null)
      splitCall = SplitNode;
    root = new DungeonNode2D(area, this);
  }

  public void Build()
  {
    root.Split(splitCall);
    foreach (DungeonNode2D node in leaves)
      node.CreateBlock();
    // Create corridors
  }

  private void Awake()
  {
    tree = new Dictionary<int, List<DungeonNode2D>>();
    leaves = new HashSet<DungeonNode2D>();
  }

  public Rect[] SplitNode(Rect area)
  {
    Rect[] areas = null;
    DungeonNode2D[] children = null;

    float value = Mathf.Min(area.width, area.height);
    if (value < minAcceptSize)
      return areas;
    
    areas = new Rect[2];
    bool isHeightMax = area.height > area.width;
    float half;
    if (isHeightMax)
    {
      half = area.height/2f;
      areas[0] = new Rect(area);
      areas[0].height = half;
      areas[1] = new Rect(area);
      areas[1].y = areas[0].y + areas[0].height;
    }
    else
    {
      half = area.width/2f;
      areas[0] = new Rect(area);
      areas[0].width = half;
      areas[1] = new Rect(area);
      areas[1].x = areas[0].x + areas[0].width;
    }
    return areas;
  }

}