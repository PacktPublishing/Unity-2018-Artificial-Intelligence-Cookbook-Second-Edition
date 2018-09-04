/*
 * File: CarController.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Created Date: 2018-05-27 16:18:50
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-05-27 16:26:33
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;

public class CarController : MonoBehaviour
{
  public float speed;
  public float maxSpeed;
  public float steering;
  public float maxSteering;
  public Vector3 velocity;

  private void Update()
  {
    transform.Rotate(Vector3.up, steering, Space.Self);
    transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
  }
}