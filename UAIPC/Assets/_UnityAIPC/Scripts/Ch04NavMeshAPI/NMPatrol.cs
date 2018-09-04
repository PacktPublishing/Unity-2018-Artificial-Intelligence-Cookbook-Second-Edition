/*
 * File: NMPatrol.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Created Date: 2018-05-13 13:37:25
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-05-13 14:00:18
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Navigation Mesh Patrol
/// </summary>
/// 

[RequireComponent(typeof(NavMeshAgent))]
public class NMPatrol : MonoBehaviour
{
  public float pointDistance = 0.5f;
  public Transform[] patrolPoints;
  private int currentPoint = 0;
  private NavMeshAgent agent;

  private void Start()
  {
    agent = GetComponent<NavMeshAgent>();
    agent.autoBraking = false;
    currentPoint = FindClosestPoint();
    GoToPoint(currentPoint);
  }

  private void Update()
  {
    if (!agent.pathPending && agent.remainingDistance < pointDistance)
      GoToPoint((currentPoint + 1) % patrolPoints.Length);
  }

  private void GoToPoint(int next)
  {
    if (next < 0 || next >= patrolPoints.Length)
      return;
    agent.destination = patrolPoints[next].position;
  }
  private int FindClosestPoint()
  {
    int index = -1;
    float distance = Mathf.Infinity;
    int i;
    Vector3 agentPosition = transform.position;
    Vector3 pointPosition;
    for (i = 0; i < patrolPoints.Length; i++)
    {
      pointPosition = patrolPoints[i].position;
      float d = Vector3.Distance(agentPosition, pointPosition);
      if (d < distance)
      {
        index = i;
        distance = d;
      }
    }
    return index;
  }
}