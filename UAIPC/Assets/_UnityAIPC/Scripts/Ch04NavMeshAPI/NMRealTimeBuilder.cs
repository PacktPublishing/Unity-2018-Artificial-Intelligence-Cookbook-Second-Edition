/*
 * File: NMRealTimeBuilder.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-07-15 23:56:16
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class NMRealTimeBuilder : MonoBehaviour
{
  public Transform agent;
  public Vector3 boxSize = new Vector3(50f, 20f, 50f);
  [Range(0.01f, 1f)]
  public float sizeChange = 0.1f;
  private NavMeshData navMesh;
  private AsyncOperation operation;
  private NavMeshDataInstance navMeshInstance;
  private List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();

  private void Awake()
  {
    if (agent == null)
      agent = transform;    
  }
  private IEnumerator Start()
  {
    for(;;)
    {
      UpdateNavMesh(true);
      yield return operation;
    }
  }
  private void OnEnable()
  {
    navMesh = new NavMeshData();
    navMeshInstance = NavMesh.AddNavMeshData(navMesh);
    UpdateNavMesh(false);
  }
  private void OnDisable()
  {
    navMeshInstance.Remove();
  }

  private void UpdateNavMesh(bool isAsync = false)
  {
    NavMeshSourceTag.Collect(ref sources);
    NavMeshBuildSettings settings;
    settings = NavMesh.GetSettingsByID(0);
    Bounds bounds = QuantizeBounds();
    if (isAsync)
      operation = NavMeshBuilder.UpdateNavMeshDataAsync(navMesh, settings, sources, bounds);
    else
      NavMeshBuilder.UpdateNavMeshData(navMesh, settings, sources, bounds);
  }

  private Bounds QuantizeBounds()
  {
    Vector3 position = agent.transform.position;
    return new Bounds(Quantize(position, boxSize * sizeChange), boxSize);
  }

  static private Vector3 Quantize(Vector3 a, Vector3 q)
  {
    float x = q.x * Mathf.Floor(a.x/q.x);
    float y = q.y * Mathf.Floor(a.y/q.y);
    float z = q.z * Mathf.Floor(a.z/q.z);
    return new Vector3(x, y, z);
  }
}