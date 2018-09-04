/*
 * File: AgentPathFollower.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 11:32:49
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */


using UnityEngine;
using System.Collections;
/// <summary>
/// Behavior based on seek for following a path
/// </summary>
public class AgentPathFollower : Seek
{
  /// <summary>
  /// Path to follow
  /// </summary>
  public AgentPath path;
  public float pathOffset = 0f;
  /// <summary>
  /// Progress overall the the path
  /// </summary>
  float currentParam;

  public override void Awake()
  {
    base.Awake();
    target = new GameObject();
    currentParam = 0f;
  }

  public override Steering GetSteering()
  {
    currentParam = path.GetParam(transform.position, currentParam);
    float targetParam = currentParam + pathOffset;
    target.transform.position = path.GetPosition(targetParam);
    // TODO
    // Change the approach in order to solve
    // Vector3 targetParam = currentParam + pathOffset;
    // target.transform.position = path.GetPosition(currentParam);
    return base.GetSteering();
  }
}
