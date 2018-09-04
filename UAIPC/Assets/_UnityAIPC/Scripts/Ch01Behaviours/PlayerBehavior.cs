/*
 * File: PlayerBehavior.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Created Date: 2018-04-22 12:59:45
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-04-22 13:04:51
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerBehavior : AgentBehavior
{
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public override Steering GetSteering()
  {
    Steering steering = new Steering();
    steering.linear.x = Input.GetAxis("Horizontal");
    steering.linear.z = Input.GetAxis("Vertical");
    steering.linear.Normalize();
    steering.linear *= agent.maxAccel;
    return steering;
  }
}