/*
 * File: Blackboard.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-07-04 22:53:55
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using System.Collections.Generic;
public class Blackboard
{
  public List<BlackboardDatum> data;
  public List<BlackboardAction> entries;
  public List<BlackboardAction> pastActions;
  public List<BlackboardExpert> experts;
  public Blackboard()
  {
    entries = new List<BlackboardAction>();
    pastActions = new List<BlackboardAction>();
    experts = new List<BlackboardExpert>();
    data = new List<BlackboardDatum>();
  }

  public void RunIteration()
  {
    BlackboardExpert bestExpert = null;
    float maxInsistence = 0f;
    foreach (BlackboardExpert e in experts)
    {
      float insistence = e.GetInsistence(this);
      if (insistence > maxInsistence)
      {
        maxInsistence = insistence;
        bestExpert = e;
      }
    }
    if (bestExpert != null)
      bestExpert.Run(this);
  }

}

public struct BlackboardAction
{
  public object expert;
  public string name;
  public System.Action action;
}