/*
 * File: DriverProfile.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Created Date: 2018-05-27 15:35:28
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-05-27 15:56:41
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;

[CreateAssetMenu(fileName = "DProfile", menuName = "UAIPC/DriverProfile", order = 0)]
public class DriverProfile : ScriptableObject
{
  [Range(0f, 1f)]
  public float skill;
  [Range(0f, 1f)]
  public float aggression;
  [Range(0f, 1f)]
  public float control;
  [Range(0f, 1f)]
  public float mistakes;
}
