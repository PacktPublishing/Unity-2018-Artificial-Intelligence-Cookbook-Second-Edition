/*
 * File: EvolEnemyGenerator.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-19 03:11:08
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Enemy generator based on evolutionary algorithm
/// </summary>
public class EvolEnemyGenerator : MonoBehaviour
{
  public int mu;
  public int lambda;
  public int generations;
  public GameObject prefab;
  public Vector2 prefabBounds;
  protected int gen;
  private int total;
  private int numAlive;
  
  public EvolEnemy[] enemyList;
  private List<EvolEnemyController> population;

  private void Start()
  {
    Init();
  }


  public void Init()
  {
    gen = 0;
    total = mu + lambda;
    population = new List<EvolEnemyController>();
    int i, x;
    bool isRandom = total != enemyList.Length;
    for (i = 0; i < enemyList.Length; i++)
    {
      EvolEnemyController enemy;
      enemy = Instantiate(prefab).GetComponent<EvolEnemyController>();
      enemy.transform.parent = transform;
      EvolEnemy template;
      x = i;
      if (isRandom)
        x = Random.Range(0, enemyList.Length - 1);
      template = enemyList[x];
      enemy.Init(template, prefabBounds);
      population.Add(enemy);
    }
    numAlive = population.Count;
    
  }

  public void CreateGeneration()
  {
    if (gen > generations)
      return;
    
    population.Sort();
    List<EvolEnemy> templateList = new List<EvolEnemy>();
    int i, x;
    for (i = mu; i < population.Count; i++)
    {
      EvolEnemy template = population[i].template;
      templateList.Add(template);
      population[i].Revive();
    }

    bool isRandom = templateList.Count != mu;
    for (i = 0; i < mu; i++)
    {
      x = i;
      if (isRandom)
        x = Random.Range(0, templateList.Count - 1);
      population[i].template = templateList[x];
      population[i].Revive();
    }
    
    gen++;
    numAlive = population.Count;
  }

  public void KillEnemy(EvolEnemyController enemy)
  {
    enemy.gameObject.SetActive(false);
    numAlive--;
    if (numAlive > 0)
      return;
    Invoke("CreateGeneration", 3f);
  }


}