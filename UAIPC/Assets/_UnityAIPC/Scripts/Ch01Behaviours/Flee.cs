/*
 * File: Flee.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 14:30:44
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */


using UnityEngine;
using System.Collections;
/// <summary>
/// Behavior for fleeing from a given point or target
/// </summary>
public class Flee : AgentBehavior {
  public override Steering GetSteering()
  {
    Steering steering = new Steering();
    steering.linear = transform.position - target.transform.position;
    steering.linear.Normalize();
    steering.linear = steering.linear * agent.maxAccel;
    return steering;
  }
}
