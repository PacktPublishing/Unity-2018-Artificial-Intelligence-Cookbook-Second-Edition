using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarkovStateMachine : MonoBehaviour
{
    public Vector4 state;
    public Matrix4x4 defaultMatrix;
    public float timeReset;
    public float timeCurrent;
    public List<MarkovTransition> transitions;
    private MonoBehaviour action;

    void Start()
    {
        timeCurrent = timeReset;
    }

    void Update()
    {
        if (action != null)
            action.enabled = false;

        MarkovTransition triggeredTransition;
        triggeredTransition = null;

        foreach (MarkovTransition mt in transitions)
        {
            if (mt.IsTriggered())
            {
                triggeredTransition = mt;
                break;
            }
        }
        if (triggeredTransition != null)
        {
            timeCurrent = timeReset;
            Matrix4x4 matrix = triggeredTransition.matrix;
            state = matrix * state;
            action = triggeredTransition.action;
        }
        else
        {
            timeCurrent -= Time.deltaTime;
            if (timeCurrent <= 0f)
            {
                state = defaultMatrix * state;
                timeCurrent = timeReset;
                action = null;
            }
        }
    }
}
