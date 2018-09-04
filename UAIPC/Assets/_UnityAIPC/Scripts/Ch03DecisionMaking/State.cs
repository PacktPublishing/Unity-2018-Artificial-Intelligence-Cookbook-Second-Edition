using UnityEngine;
using System.Collections.Generic;

public class State : MonoBehaviour
{
    public List<Transition> transitions;

    public virtual void Awake()
    {
        transitions = new List<Transition>();
        // TO-DO
        // setup your transitions here
    }
    
    public virtual void Update()
    {
        // TO-DO
        // develop behaviour here
    }

    public virtual void OnEnable()
    {
        // TO-DO
        // develop initialization here
    }

    public virtual void OnDisable()
    {
        // TO-DO
        // develop finalization here
    }


    public void LateUpdate()
    {
        foreach (Transition t in transitions)
        {
            if (t.condition.Test())
            {
                State target;
                if (t.GetType().Equals(typeof(TransitionDecision)))
                {
                    TransitionDecision td = t as TransitionDecision;
                    target = td.GetState();
                }
                else
                    target = t.target;
                target.enabled = true;
                this.enabled = false;
                return;
            }
        }
    }

}
