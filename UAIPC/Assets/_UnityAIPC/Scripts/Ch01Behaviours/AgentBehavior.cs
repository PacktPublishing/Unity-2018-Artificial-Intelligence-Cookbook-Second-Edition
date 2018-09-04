/*
 * File: AgentBehavior.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-09 16:54:33
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
/// <summary>
/// Abstract class that works as blueprint for all the agent behaviors
/// </summary>
public class AgentBehavior : MonoBehaviour
{
  /// <summary>
  /// Weight
  /// </summary>
  public float weight = 1.0f;
  /// <summary>
  /// Priority value
  /// </summary>
  public int priority = 1;
  /// <summary>
  /// Target (to apply or mimic the behavior, it depends on the behavior)
  /// </summary>
  public GameObject target;
  /// <summary>
  /// Agent that makes use of the behavior
  /// </summary>
  protected Agent agent;
  
  public virtual void Awake ()
  {
    agent = gameObject.GetComponent<Agent>();
  }
  public virtual void Update ()
  {
    if (agent.blendWeight)
      agent.SetSteering(GetSteering(), weight);
    else if (agent.blendPriority)
      agent.SetSteering(GetSteering(), priority);
    else
      agent.SetSteering(GetSteering());
  }

  /// <summary>
  /// Returns the computed steering
  /// </summary>
  /// <returns></returns>
  public virtual Steering GetSteering ()
  {
    return new Steering();
  }
  /// <summary>
  /// Maps the rotation to the 360-degree range
  /// </summary>
  /// <param name="rotation"></param>
  /// <returns></returns>
  public float MapToRange (float rotation) {
    rotation %= 360.0f;
    if (Mathf.Abs(rotation) > 180.0f) {
      if (rotation < 0.0f)
        rotation += 360.0f;
      else
        rotation -= 360.0f;
    }
    return rotation;
  }

  /// <summary>
  /// Changes the orientation float value to a Vector3 
  /// </summary>
  /// <param name="orientation"></param>
  /// <returns></returns>
  public Vector3 OriToVec (float orientation) {
    Vector3 vector  = Vector3.zero;
    vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
    vector.z = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
    return vector.normalized;
  }
}
