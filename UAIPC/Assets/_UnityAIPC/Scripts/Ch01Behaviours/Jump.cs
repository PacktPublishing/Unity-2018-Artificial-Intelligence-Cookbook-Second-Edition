/*
 * File: Jump.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 14:40:48
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */


using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Behavior for jumping based on velocity match
/// </summary>
public class Jump : VelocityMatch
{
  /// <summary>
  /// Jump point
  /// </summary>
  public JumpPoint jumpPoint;

  /// <summary>
  /// Keeps track of whether the jump is achievable
  /// </summary>
  bool canAchieve = false;

  /// <summary>
  /// Holds the maximum vertical jump velocity
  /// </summary>
  public float maxYVelocity;

  /// <summary>
  /// Definition for gravity
  /// </summary>
  /// <returns></returns>
  public Vector3 gravity = new Vector3(0, -9.8f, 0);

  /// <summary>
  /// Projectile behavior
  /// </summary>
  private Projectile projectile;

  /// <summary>
  /// List of behaviors attached to the object
  /// </summary>
  private List<AgentBehavior> behaviours;


  public void Isolate(bool state)
  {
    // disable all the behaviors
    foreach (AgentBehavior b in behaviours)
      b.enabled = !state;
    this.enabled = state;
  }

  public void DoJump()
  {
    projectile.enabled = true;
    Vector3 direction;
    direction = Projectile.GetFireDirection(jumpPoint.jumpLocation, jumpPoint.landingLocation, agent.maxSpeed);
    projectile.Set(jumpPoint.jumpLocation, direction, agent.maxSpeed, false);
  }

  public override void Awake()
  {
    base.Awake();
    this.enabled = false;
    projectile = gameObject.AddComponent<Projectile>();
    behaviours = new List<AgentBehavior>();
    AgentBehavior[] abs;
    abs = gameObject.GetComponents<AgentBehavior>();
    foreach (AgentBehavior b in abs)
    {
      if (b == this)
        continue;
      behaviours.Add(b);
    }
  }


  public override Steering GetSteering()
  {
    Steering steering = new Steering();

    // Check if we have a trajectory, and create one if not.
    if (jumpPoint != null && target == null)
    {
      CalculateTarget();
    }

    //Check if the trajectory is zero. If not, we have no acceleration.
    if (!canAchieve)
    {
      return steering;
    }

    //Check if we’ve hit the jump point
    if (Mathf.Approximately((transform.position - target.transform.position).magnitude, 0f) &&
      Mathf.Approximately((agent.velocity - target.GetComponent<Agent>().velocity).magnitude, 0f))
    {
      DoJump();
      return steering;
    }

    return base.GetSteering();
  }

  protected void CalculateTarget()
  {
    target = new GameObject();
    target.AddComponent<Agent>();

    //Calculate the first jump time
    float sqrtTerm = Mathf.Sqrt(2f * gravity.y * jumpPoint.deltaPosition.y + maxYVelocity * agent.maxSpeed);

    float time = (maxYVelocity - sqrtTerm) / gravity.y;

    //Check if we can use it, otherwise try the other time
    if (!CheckJumpTime(time))
    {
      time = (maxYVelocity + sqrtTerm) / gravity.y;
    }
  }

  //Private helper method for the CalculateTarget function
  private bool CheckJumpTime(float time)
  {
    //Calculate the planar speed
    float vx = jumpPoint.deltaPosition.x / time;
    float vz = jumpPoint.deltaPosition.z / time;

    float speedSq = vx * vx + vz * vz;

    //Check it to see if we have a valid solution
    if (speedSq < agent.maxSpeed * agent.maxSpeed)
    {
      target.GetComponent<Agent>().velocity = new Vector3(vx, 0f, vz);
      canAchieve = true;
      return true;
    }

    return false;
  }
}
