/*
 * File: Face.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 14:29:56
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// Behavior for facing to a certain direction based on a target object
/// </summary>
public class Face : Align
{
  /// <summary>
  /// Auxiliary target to align with
  /// </summary>
  protected GameObject targetAux;

  public override void Awake()
  {
    base.Awake();
    targetAux = target;
    target = new GameObject();
    target.AddComponent<Agent>();
}

  public override Steering GetSteering()
  {
    Vector3 direction = targetAux.transform.position - transform.position;
    if (direction.magnitude > 0.0f)
    {
      float targetOrientation = Mathf.Atan2(direction.x, direction.z);
      targetOrientation *= Mathf.Rad2Deg;
      target.GetComponent<Agent>().orientation = targetOrientation;
    }
    return base.GetSteering();
  }

  void OnDestroy ()
  {
    Destroy(target);
  }
}
