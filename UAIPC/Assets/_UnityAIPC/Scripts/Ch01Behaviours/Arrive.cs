/*
 * File: Arrive.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 14:22:50
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */


using UnityEngine;
using System.Collections;

/// <summary>
/// Behavior for arriving at a point
/// </summary>
public class Arrive : AgentBehavior
{
  /// <summary>
  /// Radius for stopping
  /// </summary>  
  public float targetRadius;
  /// <summary>
  /// Radius for starting to slow down
  /// </summary>
  public float slowRadius;
  public float timeToTarget = 0.1f;

  public override Steering GetSteering()
  {
    Steering steering = new Steering();
    Vector3 direction = target.transform.position - transform.position;
    float distance = direction.magnitude;
    float targetSpeed;
    if (distance < targetRadius)
      return steering;
    if (distance > slowRadius)
      targetSpeed = agent.maxSpeed;
    else
      targetSpeed = agent.maxSpeed * distance / slowRadius;
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
