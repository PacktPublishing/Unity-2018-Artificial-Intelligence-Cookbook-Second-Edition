using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateHighLevel : State
{
    public List<State> states;
    public State stateInitial;
    protected State stateCurrent;

    public override void OnEnable()
    {
        if (stateCurrent == null)
            stateCurrent = stateInitial;
        stateCurrent.enabled = true;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        stateCurrent.enabled = false;
        foreach (State s in states)
        {
            s.enabled = false;
        }
    }
}
