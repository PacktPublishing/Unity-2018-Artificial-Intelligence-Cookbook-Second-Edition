/*
 * File: TrackNode.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Created Date: 2018-05-27 15:59:24
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-05-27 16:43:11
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;

public class TrackNode : MonoBehaviour
{
  public float raceWidth;
  public float offWidth;
  public float wallWidth;
  
  public TrackNode prev;
  public TrackNode next;
  public Vector3 normal;

  private void Awake()
  {
    normal = transform.forward;
    if (prev != null && next != null)
    {
      Vector3 nextPosition, prevPosition;
      nextPosition = next.transform.position;
      prevPosition = prev.transform.position;
      normal = nextPosition - transform.position;
      normal += transform.position - prevPosition;
      normal /= 2f;
      normal.Normalize();
    }
  }
}