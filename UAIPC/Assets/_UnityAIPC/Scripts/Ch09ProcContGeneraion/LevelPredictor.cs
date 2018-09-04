/*
 * File: LevelPredictor.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-17 22:53:22
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

public class LevelPredictor : NGramPredictor<LevelSlice>
{
  public LevelPredictor(int windowSize) : base(windowSize)
  {
  }
}