/*
 * File: AgentPlayer.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 14:19:30
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */


using UnityEngine;
/// <summary>
/// Class for modeling the player controller as an agent
/// </summary>
public class AgentPlayer : Agent
{
  /// <summary>
  /// The rigid body component
  /// </summary>
  private Rigidbody _rigidBody;
  /// <summary>
  /// Direction of movement
  /// </summary>
  private Vector3 _direction;

  private void Awake()
  {
    _rigidBody = GetComponent<Rigidbody>();
  }

  public override void Update()
  {
    velocity.x = Input.GetAxis("Horizontal");
    velocity.z = Input.GetAxis("Vertical");
    velocity *= maxSpeed;
    //Vector3 translation = velocity * Time.deltaTime;
  }

  public override void FixedUpdate()
  {
    if (_rigidBody == null)
    {
      transform.Translate(velocity * Time.deltaTime, Space.World);
    }
    else
    {
      _rigidBody.AddForce(velocity * Time.deltaTime, ForceMode.VelocityChange);
    }        
    //orientation = transform.rotation.eulerAngles.y;
  }

  public override void LateUpdate()
  {
    transform.LookAt(transform.position + velocity);
  }
}
