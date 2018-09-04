/*
 * File: Agent.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-09 16:29:35
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */


using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// The Agent class is responsible of modeling the agent
/// and handling all the behaviors attached for blending
/// them (if available)
/// </summary>
public class Agent : MonoBehaviour
{
  /// <summary>
  /// Blend by weight
  /// </summary>
  public bool blendWeight = false;
  /// <summary>
  /// Blend by priority
  /// </summary>
  public bool blendPriority = false;
  /// <summary>
  /// Priority threshold for taking the value into account
  /// </summary>
  public float priorityThreshold = 0.2f;
  /// <summary>
  /// Maximum speed
  /// </summary>
  public float maxSpeed;
  /// <summary>
  /// Maximum acceleration
  /// </summary>
  public float maxAccel;
  /// <summary>
  /// Maximum rotation
  /// </summary>
  public float maxRotation;
  /// <summary>
  /// Maximum angular acceleration
  /// </summary>
  public float maxAngularAccel;
  /// <summary>
  /// Orientation (it's like angular velocity)
  /// </summary>
  public float orientation;
  /// <summary>
  /// Rotation (variable value, just like speed, for changing orientation)
  /// </summary>
  public float rotation;
  /// <summary>
  /// Velocity
  /// </summary>
  public Vector3 velocity;
  /// <summary>
  /// Steering value
  /// </summary>
  protected Steering steering;
  /// <summary>
  /// Steering groups, grouped by priority value
  /// </summary>
  private Dictionary<int, List<Steering>> groups;
  /// <summary>
  /// Rigidbody Component
  /// </summary>
  private Rigidbody aRigidBody;

	void Start ()
  {
    velocity = Vector3.zero;
    steering = new Steering();
    groups = new Dictionary<int, List<Steering>>();
    aRigidBody = GetComponent<Rigidbody>();
	}


  public virtual void FixedUpdate()
  {
    if (aRigidBody == null)
      return;

    Vector3 displacement = velocity * Time.deltaTime;
    orientation += rotation * Time.deltaTime;
    // we need to smart clamp the orientation
    // to the range (0, 360)
    if (orientation < 0.0f)
      orientation += 360.0f;
    else if (orientation > 360.0f)
      orientation -= 360.0f;
    // The ForceMode will depend on what you want to achieve
    // We are using VelocityChange for illustration purposes
    aRigidBody.AddForce(displacement, ForceMode.VelocityChange);
    Vector3 orientationVector = OriToVec(orientation);
    aRigidBody.rotation = Quaternion.LookRotation(orientationVector, Vector3.up);
  }

	public virtual void Update ()
  {
    if (aRigidBody != null)
      return;
    // ... previous code
    Vector3 displacement = velocity * Time.deltaTime;
    orientation += rotation * Time.deltaTime;
    // we need to smart clamp the orientation
    // to the range (0, 360)
    if (orientation < 0.0f)
      orientation += 360.0f;
    else if (orientation > 360.0f)
      orientation -= 360.0f;
    transform.Translate(displacement, Space.World);
    // restore rotation to initial point
    // before rotating the object to our value
    transform.rotation = new Quaternion();
    transform.Rotate(Vector3.up, orientation);
	}

  public virtual void LateUpdate ()
  {
    if (blendPriority)
    {
      steering = GetPrioritySteering();
      groups.Clear();
    }
    velocity += steering.linear * Time.deltaTime;
    rotation += steering.angular * Time.deltaTime;
    if (velocity.magnitude > maxSpeed)
    {
      velocity.Normalize();
      velocity = velocity * maxSpeed;
    }
    if (rotation > maxRotation)
    {
      rotation = maxRotation;
    }
    if (steering.angular == 0.0f)
    {
      rotation = 0.0f;
    }
    if (steering.linear.sqrMagnitude == 0.0f)
    {
      velocity = Vector3.zero;
    }
    steering = new Steering();
  }

  public void SetSteering (Steering steering)
  {
    this.steering = steering;
  }

  /// <summary>
  /// Sets the steering by weight
  /// </summary>
  /// <param name="steering"></param>
  /// <param name="weight"></param>
  public void SetSteering (Steering steering, float weight)
  {
    this.steering.linear += (weight * steering.linear);
    this.steering.angular += (weight * steering.angular);
  }

  /// <summary>
  /// Sets the steering by priority
  /// </summary>
  /// <param name="steering"></param>
  /// <param name="priority"></param>
  public void SetSteering (Steering steering, int priority)
  {
    if (!groups.ContainsKey(priority))
    {
      groups.Add(priority, new List<Steering>());
    }
    groups[priority].Add(steering);
  }

  /// <summary>
  /// Returns the steering value calculated by priority
  /// </summary>
  /// <returns></returns>
  private Steering GetPrioritySteering ()
  {
    Steering steering = new Steering();
    List<int> gIdList = new List<int>(groups.Keys);
    gIdList.Sort();
    foreach (int gid in gIdList)
    {
      steering = new Steering();
      foreach (Steering singleSteering in groups[gid])
      {
        steering.linear += singleSteering.linear;
        steering.angular += singleSteering.angular;
      }
      if (steering.linear.magnitude > priorityThreshold
           || Mathf.Abs(steering.angular) > priorityThreshold)
      {
        return steering;
      }
    }
    return steering;
  }
  /// <summary>
  /// Calculates the Vector3 given orientation value
  /// </summary>
  /// <param name="orientation"></param>
  /// <returns></returns>
  public Vector3 OriToVec(float orientation)
  {
    Vector3 vector = Vector3.zero;
    vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
    vector.z = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
    return vector.normalized;
  }
}
