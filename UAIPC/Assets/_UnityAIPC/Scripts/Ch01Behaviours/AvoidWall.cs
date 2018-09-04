/*
 * File: AvoidWall.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 14:25:51
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// Behavior for avoiding walls
/// </summary>
public class AvoidWall : Seek
{
  /// <summary>
  /// Collision avoidance distance
  /// </summary>
  public float avoidDistance;
  /// <summary>
  /// Lenght of ray for looking ahead and avoid walls
  /// </summary>
  public float lookAhead;

  public override void Awake()
  {
    base.Awake();
    target = new GameObject();
  }

  public override Steering GetSteering()
  {
    Steering steering = new Steering();
    Vector3 position = transform.position;
    Vector3 rayVector = agent.velocity.normalized * lookAhead;
    rayVector += position;;
    Vector3 direction = rayVector - position;
    RaycastHit hit;
    if (Physics.Raycast(position, direction, out hit, lookAhead))
    {
      position = hit.point + hit.normal * avoidDistance;
      target.transform.position = position;
      steering = base.GetSteering();
    }
    return steering;
  }
}
