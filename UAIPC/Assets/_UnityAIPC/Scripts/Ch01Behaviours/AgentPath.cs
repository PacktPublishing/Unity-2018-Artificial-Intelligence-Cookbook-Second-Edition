/*
 * File: AgentPath.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-09 17:07:02
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Defines a path for an agent to travel between points
/// </summary>
public class AgentPath : MonoBehaviour
{
  /// <summary>
  /// List of nodes
  /// </summary>
  public List<GameObject> nodes;
  /// <summary>
  /// Path segments
  /// </summary>
  List<AgentPathSegment> segments;


  void Start()
  {
    segments = GetSegments();
  }

  void OnDrawGizmos ()
  {
    Vector3 direction;
    Color tmp = Gizmos.color;
    Gizmos.color = Color.magenta;
    int i;
    for (i = 0; i < nodes.Count - 1; i++)
    {
      Vector3 src = nodes[i].transform.position;
      Vector3 dst = nodes[i+1].transform.position;
      direction = dst - src;
      Gizmos.DrawRay(src, direction);
    }
    Gizmos.color = tmp;
  }

  /// <summary>
  /// Retrieves the nearest segment to the given position
  /// </summary>
  /// <param name="position"></param>
  /// <returns></returns>
  private AgentPathSegment GetNearestSegment (Vector3 position) {
    float nearestDistance = Mathf.Infinity;
    float distance = nearestDistance;
    Vector3 centroid = Vector3.zero;
    AgentPathSegment segment = new AgentPathSegment();
    foreach (AgentPathSegment s in segments)
    {
      centroid = (s.a + s.b) / 2.0f;
      distance = Vector3.Distance(position, centroid);
      if (distance < nearestDistance)
      {
        nearestDistance = distance;
        segment = s;
      }
    }
    return segment;
  }

  /// <summary>
  /// Retrieves the list of all segments
  /// </summary>
  /// <returns></returns>
  public List<AgentPathSegment> GetSegments () {
    List<AgentPathSegment> segments = new List<AgentPathSegment>();
    int i;
    for (i = 0; i < nodes.Count - 1; i++)
    {
      Vector3 src = nodes[i].transform.position;
      Vector3 dst = nodes[i+1].transform.position;
      AgentPathSegment segment = new AgentPathSegment(src, dst);
      segments.Add(segment);
    }
    return segments;
  }

  /// <summary>
  /// Retrieves the param according to the position and last param used
  /// </summary>
  /// <param name="position"></param>
  /// <param name="lastParam"></param>
  /// <returns></returns>
  public float GetParam(Vector3 position, float lastParam) {
    float param = 0f;

    AgentPathSegment currentSegment = null;
    float tempParam = 0f;

    //We find the current segment in the path where the agent is
    foreach (AgentPathSegment ps in segments)
    {
      tempParam += Vector3.Distance(ps.a, ps.b);
      if (lastParam <= tempParam)
      {
        currentSegment = ps;
        break;
      }
    }

    if (currentSegment == null)
      return 0f;

    Vector3 segmentDirection = currentSegment.b - currentSegment.a;
    segmentDirection.Normalize();
    Vector3 currPos = position - currentSegment.a;
    //We use vector projections to find the point over the segment
    Vector3 pointInSegment = Vector3.Project(currPos, segmentDirection);
    //The current param is the sum of distances until the last node
    //plus the length of the projection from last step
    param = tempParam - Vector3.Distance(currentSegment.a, currentSegment.b);
    param += pointInSegment.magnitude;
    return param;
  }

  /// <summary>
  /// Retrieves the position relative to the param
  /// </summary>
  /// <param name="param"></param>
  /// <returns></returns>
  public Vector3 GetPosition(float param) 
  {
    Vector3 position = Vector3.zero;

    AgentPathSegment currentSegment = null;
    float tempParam = 0f;

    //We find the current segment in the path where the agent is
    foreach (AgentPathSegment ps in segments)
    {
      tempParam += Vector3.Distance(ps.a, ps.b);
      if (param <= tempParam)
      {
        currentSegment = ps;
        break;
      }
    }

    if (currentSegment == null)
      return Vector3.zero;

    Vector3 segmentDirection = currentSegment.b - currentSegment.a;
    segmentDirection.Normalize();
    tempParam -= Vector3.Distance(currentSegment.a, currentSegment.b);
    tempParam = param - tempParam;
    position = currentSegment.a + segmentDirection * tempParam;
    return position;
  }
}
