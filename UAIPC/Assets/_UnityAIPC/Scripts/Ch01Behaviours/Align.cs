/*
 * File: Align.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 14:22:01
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// Align behavior
/// </summary>
public class Align : AgentBehavior
{
  /// <summary>
  /// Radius for stopping
  /// </summary>
  public float targetRadius;
  /// <summary>
  /// Radius for slowing down
  /// </summary>
  public float slowRadius;
  public float timeToTarget = 0.1f;

  public override Steering GetSteering()
  {
    Steering steering = new Steering();
    float targetOrientation = target.GetComponent<Agent>().orientation;
    float rotation = targetOrientation - agent.orientation;
    rotation = MapToRange(rotation);
    float rotationSize = Mathf.Abs(rotation);
    if (rotationSize < targetRadius)
        return steering;
    float targetRotation;
    if (rotationSize > slowRadius)
      targetRotation = agent.maxRotation;
    else
      targetRotation = agent.maxRotation * rotationSize / slowRadius;
    targetRotation *= rotation / rotationSize;
    steering.angular = targetRotation - agent.rotation;
    steering.angular /= timeToTarget;
    float angularAccel = Mathf.Abs(steering.angular);
    if (angularAccel > agent.maxAngularAccel)
    {
      steering.angular /= angularAccel;
      steering.angular *= agent.maxAngularAccel;
    }
    return steering;
  }
}
