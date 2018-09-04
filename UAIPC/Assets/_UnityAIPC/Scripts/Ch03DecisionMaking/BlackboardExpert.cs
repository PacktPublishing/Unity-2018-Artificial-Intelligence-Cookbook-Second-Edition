/*
 * File: BlackboardExpert.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-07-04 22:23:35
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

public abstract class BlackboardExpert
{
  public virtual float GetInsistence(Blackboard board)
  {
    return 0f;
  }
  public virtual void Run(Blackboard board){}
}