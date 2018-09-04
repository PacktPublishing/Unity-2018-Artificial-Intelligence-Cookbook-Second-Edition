/*
 * File: JumpLocation.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 14:53:25
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
/// <summary>
/// Defines a jump location to an object
/// </summary>
public class JumpLocation : MonoBehaviour
{
  /// <summary>
  /// Landing point
  /// </summary>
  public LandingLocation landingLocation;

  public void OnTriggerEnter(Collider other)
  {
    if (!other.gameObject.CompareTag("Agent"))
      return;
    Agent agent = other.GetComponent<Agent>();
    Jump jump = other.GetComponent<Jump>();
    if (agent == null || jump == null)
      return;
    Vector3 originPos = transform.position;
    Vector3 targetPos = landingLocation.transform.position;
    jump.Isolate(true);
    jump.jumpPoint = new JumpPoint(originPos, targetPos);
    jump.DoJump();
  }
}
