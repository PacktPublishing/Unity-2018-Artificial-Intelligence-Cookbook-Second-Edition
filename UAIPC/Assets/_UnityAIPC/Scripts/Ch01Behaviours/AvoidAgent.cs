/*
 * File: AvoidAgent.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 14:24:33
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Behavior for avoiding other agents
/// </summary>
public class AvoidAgent : AgentBehavior
{
  /// <summary>
  /// Threshold for avoiding collision
  /// </summary>
  public float collisionRadius = 0.4f;
  /// <summary>
  /// Other agents' game object for avoiding collision
  /// </summary>
  GameObject[] targets;

  void Start ()
  {
    targets = GameObject.FindGameObjectsWithTag("Agent");
  }

  public override Steering GetSteering()
  {
    Steering steering = new Steering();
    float shortestTime = Mathf.Infinity;
    GameObject firstTarget = null;
    float firstMinSeparation = 0.0f;
    float firstDistance = 0.0f;
    Vector3 firstRelativePos = Vector3.zero;
    Vector3 firstRelativeVel = Vector3.zero;
    foreach (GameObject t in targets)
    {
      Vector3 relativePos;
      Agent targetAgent = t.GetComponent<Agent>();
      relativePos = t.transform.position - transform.position;
      Vector3 relativeVel = targetAgent.velocity - agent.velocity;
      float relativeSpeed = relativeVel.magnitude;
      float timeToCollision = Vector3.Dot(relativePos, relativeVel);
      timeToCollision /= relativeSpeed * relativeSpeed * -1;
      float distance = relativePos.magnitude;
      float minSeparation = distance - relativeSpeed * timeToCollision;
      if (minSeparation > 2 * collisionRadius)
          continue;
      if (timeToCollision > 0.0f && timeToCollision < shortestTime)
      {
        shortestTime = timeToCollision;
        firstTarget = t;
        firstMinSeparation = minSeparation;
        firstRelativePos = relativePos;
        firstRelativeVel = relativeVel;
      }
    }
    if (firstTarget == null)
      return steering;
    if (firstMinSeparation <= 0.0f || firstDistance < 2 * collisionRadius)
      firstRelativePos = firstTarget.transform.position;
    else
      firstRelativePos += firstRelativeVel * shortestTime;
    firstRelativePos.Normalize();
    steering.linear = -firstRelativePos * agent.maxAccel;
    return steering;
  }
}
