/*
 * File: EvolEnemyController.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-17 19:33:14
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System;
using System.Collections;

public class EvolEnemyController :
    MonoBehaviour, IComparable<EvolEnemyController>
{
  public static int counter = 0;
  [HideInInspector]
  public EvolEnemy template;
  public float time;
  protected Vector2 bounds;
  protected SpriteRenderer _renderer;
  protected BoxCollider2D _collider;

  public void Init(EvolEnemy template, Vector2 bounds)
  {
    this.template = template;
    this.bounds = bounds;
    Revive();
  }

  public void Revive()
  {
    gameObject.SetActive(true);
    counter++;
    gameObject.name = "EvolEnemy" + counter;

    Vector3 newPosition = UnityEngine.Random.insideUnitCircle;
    newPosition *= bounds;
    _renderer.sprite = template.sprite;
    _collider = gameObject.AddComponent<BoxCollider2D>();
  }

  public int CompareTo(EvolEnemyController other)
  {
    return other.time > time ? 0 : 1;
  }

  private void Update()
  {
    if (template == null)
      return;
    time += Time.deltaTime;  
  }

  private void OnMouseDown()
  {
    Destroy(_collider);
    gameObject.SendMessageUpwards("KillEnemy", this);
  }

  
}