/*
 * File: LevelGenerator.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-17 22:52:34
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
  public LevelPredictor predictor;
  public List<LevelSlice> pattern;
  public List<LevelSlice> result;
  private bool isInit;

  private void Start()
  {
    isInit = false;
  }

  public void Init()
  {
    result = new List<LevelSlice>();
    predictor = new LevelPredictor(3);
    predictor.RegisterSequence(pattern.ToArray());
  }

  public void Build()
  {
    if (isInit)
      return;
    int i;
    for (i = 0; i < pattern.Count - 1; i++)
    {
      LevelSlice slice;
      LevelSlice[] input = pattern.GetRange(0, i + 1).ToArray();
      slice = predictor.GetMostLikely(input);
      result.Add(slice);
    }
  }
}