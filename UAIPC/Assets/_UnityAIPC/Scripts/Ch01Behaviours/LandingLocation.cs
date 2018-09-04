/*
 * File: LandingLocation.cs
 * Project: Unity 2018 AI Programming Cookbook
 * Author: Jorge Palacios
 * -----
 * Last Modified: 2018-06-10 16:52:24
 * Modified By: Jorge Palacios
 * -----
 * Copyright (c) 2018 Packt Publishing Ltd
 */

using UnityEngine;
/// <summary>
/// Landing location
/// </summary>
public class LandingLocation : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Agent"))
            return;
        Agent agent = other.GetComponent<Agent>();
        Jump jump = other.GetComponent<Jump>();
        if (agent == null || jump == null)
            return;
        jump.Isolate(false);
        jump.jumpPoint = null;
    }
}
