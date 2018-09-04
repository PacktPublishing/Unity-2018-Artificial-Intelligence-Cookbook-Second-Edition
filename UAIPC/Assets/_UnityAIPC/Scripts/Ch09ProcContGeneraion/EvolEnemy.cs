/*
 * File: EvolEnemy.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-17 17:36:18
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EvolEnemy
{
  public Sprite sprite;
  public int healthInit;
  public int healthMax;
  public int healthVariance;
}