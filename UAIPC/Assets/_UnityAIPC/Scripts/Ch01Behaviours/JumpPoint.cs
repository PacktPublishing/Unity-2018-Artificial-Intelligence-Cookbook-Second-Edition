/*
 * File: JumpPoint.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 16:51:52
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
/// <summary>
/// Defines a jump point
/// </summary>
public class JumpPoint 
{
  /// <summary>
  /// Jump location
  /// </summary>
  public Vector3 jumpLocation;
  /// <summary>
  /// Landing location
  /// </summary>
  public Vector3 landingLocation;
  /// <summary>
  /// The change in position from jump to landing
  /// </summary>
  public Vector3 deltaPosition;

  public JumpPoint ()
    : this (Vector3.zero, Vector3.zero)
  {
  }

  public JumpPoint(Vector3 a, Vector3 b)
  {
    this.jumpLocation = a;
    this.landingLocation = b;
    this.deltaPosition = this.landingLocation - this.jumpLocation;
  }
}
