/*
 * File: UCB1.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-07-05 21:10:42
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;

public enum RPSAction
{
  Rock, Paper, Scissors
}

public class Bandit : MonoBehaviour
{
  bool init;
  int totalActions;
  int[] count;
  float[] score;
  int numActions;
  RPSAction lastAction;
  int lastStrategy;

  // REGRET MATCHING EXTRAS
  float initialRegret = 10f;
  float[] regret;
  float[] chance;
  RPSAction lastOpponentAction;
  RPSAction[] lastActionRM;

  public void InitUCB1()
  {
    if (init)
      return;
    totalActions = 0;
    numActions = System.Enum.GetNames(typeof(RPSAction)).Length;
    count = new int[numActions];
    score = new float[numActions];
    int i;
    for (i = 0; i < numActions; i++)
    {
      count[i] = 0;
      score[i] = 0f;
    }
    init = true;
  }


  public RPSAction GetNextActionUCB1()
  {
    int i, best;
    float bestScore;
    float tempScore;
    InitUCB1();
    for (i = 0; i < numActions; i++)
    {
      if (count[i] == 0)
      {
        lastStrategy = i;
        lastAction = GetActionForStrategy((RPSAction)i);
        return lastAction;
      }
    }
    best = 0;
    bestScore = score[best]/(float)count[best];
    float input = Mathf.Log(totalActions/(float)count[best]);
    input *= 2f;
    bestScore += Mathf.Sqrt(input);
    for (i = 0; i < numActions; i++)
    {
      tempScore = score[i]/(float)count[i];
      input = Mathf.Log(totalActions/(float)count[best]);
      input *= 2f;
      tempScore = Mathf.Sqrt(input);
      if (tempScore > bestScore)
      {
        best =i;
        bestScore = tempScore;
      }
    }
    lastStrategy = best;
    lastAction = GetActionForStrategy((RPSAction)best);
    return lastAction;
  }

  public RPSAction GetActionForStrategy(RPSAction strategy)
  {
    RPSAction action;
    switch (strategy)
    {
      default:
      case RPSAction.Paper:
        action = RPSAction.Scissors;
        break;
      case RPSAction.Rock:
        action = RPSAction.Paper;
        break;
      case RPSAction.Scissors:
        action = RPSAction.Rock;
        break;
    }
    return action;
  }

  public void TellOpponentAction(RPSAction action)
  {
    totalActions++;
    float utility;
    utility = GetUtility(lastAction, action);
    score[(int)lastAction] += utility;
    count[(int)lastAction] += 1;
  }

  public float GetUtility(RPSAction myAction, RPSAction opponents)
  {
    float utility = 0f;
    if (opponents == RPSAction.Paper)
    {
      if (myAction == RPSAction.Rock)
        utility = -1f;
      else if (myAction == RPSAction.Scissors)
        utility = 1f;
    }
    else if (opponents == RPSAction.Rock)
    {
      if (myAction == RPSAction.Paper)
        utility = 1f;
      else if (myAction == RPSAction.Scissors)
        utility = -1f;
    }
    else
    {
      if (myAction == RPSAction.Rock)
        utility = -1f;
      else if (myAction == RPSAction.Paper)
        utility = 1f;
    }
    return utility;
  }


  public void InitRegretMatching()
  {
    if (init)
      return;
    numActions = System.Enum.GetNames(typeof(RPSAction)).Length;
    regret = new float[numActions];
    chance = new float[numActions];
    int i;
    for (i = 0; i < numActions; i++)
    {
      regret[i] = initialRegret;
      chance[i] = 0f;
    }
    init = true;
  }

  public RPSAction GetNextActionRM()
  {
    InitRegretMatching();
    int i;
    for (i = 0; i < numActions; i++)
    {
      lastActionRM[i] = GetActionForStrategy((RPSAction)i);
    }
    float sum = 0f;
    for (i = 0; i < numActions; i++)
    {
      if (regret[i] > 0f)
        sum += regret[i];
    }
    if (sum <= 0f)
    {
      lastAction = (RPSAction)Random.Range(0, numActions);
      return lastAction;
    }
    for (i = 0; i < numActions; i++)
    {
      chance[i] = 0f;
      if (regret[i] > 0f)
        chance[i] = regret[i];
      if (i > 0)
        chance[i] += chance[i-1];
    }
    float prob = Random.value;
    for (i = 0; i < numActions; i++)
    {
      if (prob < chance[i])
      {
        lastStrategy = i;
        lastAction = lastActionRM[i];
        return lastAction;
      }
    }
    return (RPSAction)(numActions-1);
  }

  public void TellOpponentActionRM(RPSAction action)
  {
    lastOpponentAction = action;
    int i;
    for (i = 0; i < numActions; i++)
    {
      regret[i] += GetUtility((RPSAction)lastActionRM[i], (RPSAction)action);
      regret[i] -= GetUtility((RPSAction)lastAction, (RPSAction) action);
    }
  }
}