/*
 * File: TerrainGenerator
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-14 14:16:19
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;

/// <summary>
/// Diamon-Square implementation
/// </summary>
public class TerrainGenerator : MonoBehaviour
{
  [Range(3, 101)]
  public int size;
  [Range(0.1f, 20f)]
  public float maxHeight;

  protected float[,] terrain;

  public void Init()
  {
    // It's required to be 2^n+1
    if (size % 2 == 0)
      size++;
    terrain = new float[size, size];
    terrain[0, 0] = Random.value;
    terrain[0, size-1] = Random.value;
    terrain[size-1, 0] = Random.value;
    terrain[size-1, size-1] = Random.value;
  }

  /// <summary>
  /// Diamond-Square algorithm
  /// </summary>
  public void Build()
  {
    int step = size - 1;
    float height = maxHeight;
    float r = Random.Range(0, height);
    
    // SQUARE
    for (int sideLength = size-1; sideLength >= 2; sideLength /= 2)
    {
      int half = size / 2;
      int x, y;
      for (y = 0; y < size - 1; y += sideLength)
      {
        for (x = 0; x < size -1; x += sideLength)
        {
          float average = terrain[y,x];
          average += terrain[x + sideLength, y];
          average += terrain[x, y + sideLength];
          average += terrain[x + sideLength, y + sideLength];
          average /= 4f;

          average += Random.value * 2f * height;
          terrain[y + half, x + half] = average;

        }
      }

      // DIAMOND
      for (int j = 0; j < size - 1; j = half)
      {
        for (int i = (j + half)%sideLength; i < size - 1; i += sideLength)
        {
          float average = terrain[(j-half+size)%size, i];
          average += terrain[(j+half)%size,i];
          average += terrain[j, (i+half)%size];
          average += terrain[j,(j-half+size)%size];

          average = average + (Random.value * 2f * height) - height;
          terrain[j, i] = average;

          // wrap values on the edges
          if (i == 0)
            terrain[j, size - 1] = average;
          if (j == 0)
            terrain[size-1, i] = average;
        }
      }

      height /= 2f;
    }

  }

}