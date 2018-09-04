/*
 * File: AgentPathSegment.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 14:18:36
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System.Collections;
/// <summary>
/// Path segment
/// </summary>
public class AgentPathSegment
{
  /// <summary>
  /// Start point
  /// </summary>
  public Vector3 a;
  /// <summary>
  /// End point
  /// </summary>
  public Vector3 b;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <returns></returns>
  public AgentPathSegment () : this (Vector3.zero, Vector3.zero){}

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="a"></param>
  /// <param name="b"></param>
  public AgentPathSegment (Vector3 a, Vector3 b)
  {
    this.a = a;
    this.b = b;
  }
}