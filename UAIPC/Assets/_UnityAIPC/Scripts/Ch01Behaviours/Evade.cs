/*
 * File: Evade.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 14:28:06
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
/// <summary>
/// Behavior for evading other agents.
/// It's an improvement from flee
/// </summary>
public class Evade : Flee
{
  /// <summary>
  /// Prediction threshold
  /// </summary>
  public float maxPrediction;
  /// <summary>
  /// The real target to flee from
  /// </summary>
  private GameObject targetAux;
  /// <summary>
  /// The agent to evade and get information
  /// </summary>
  private Agent targetAgent;

  public override void Awake()
  {
    base.Awake();
    targetAgent = target.GetComponent<Agent>();
    targetAux = target;
    target = new GameObject();
  }

  public override Steering GetSteering()
  {
    Vector3 direction = targetAux.transform.position - transform.position;
    float distance = direction.magnitude;
    float speed = agent.velocity.magnitude;
    float prediction;
    if (speed <= distance / maxPrediction)
        prediction = maxPrediction;
    else
        prediction = distance / speed;
    target.transform.position = targetAux.transform.position;
    target.transform.position += targetAgent.velocity * prediction;
    return base.GetSteering();
  }

  void OnDestroy ()
  {
    Destroy(targetAux);
  }
}
