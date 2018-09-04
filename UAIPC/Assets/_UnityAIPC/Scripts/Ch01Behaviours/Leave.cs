/*
 * File: Leave.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 16:55:14
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System.Collections;
/// <summary>
/// Leave behavior. It "escapes" from a given point
/// </summary>
public class Leave : AgentBehavior
{
  /// <summary>
  /// High-speed radius to escape
  /// </summary>
  public float escapeRadius;
  /// <summary>
  /// Stopping radius
  /// </summary>
  public float dangerRadius;
  public float timeToTarget = 0.1f;

  public override Steering GetSteering()
  {
    Steering steering = new Steering();
    Vector3 direction = transform.position - target.transform.position;
    float distance = direction.magnitude;
    if (distance > dangerRadius)
      return steering;
    float reduce;
    if (distance < escapeRadius)
      reduce = 0f;
    else
      reduce = distance / dangerRadius * agent.maxSpeed;
    float targetSpeed = agent.maxSpeed - reduce;
    Vector3 desiredVelocity = direction;
    desiredVelocity.Normalize();
    desiredVelocity *= targetSpeed;
    steering.linear = desiredVelocity - agent.velocity;
    steering.linear /= timeToTarget;
    if (steering.linear.magnitude > agent.maxAccel)
    {
      steering.linear.Normalize();
      steering.linear *= agent.maxAccel;
    }
    return steering;
  }
}
