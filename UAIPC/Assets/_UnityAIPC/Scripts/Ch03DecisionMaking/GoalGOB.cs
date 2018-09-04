using UnityEngine;
using System.Collections;

public class GoalGOB
{
    public string name;
    public float value;
    public float change;
    
    public virtual float GetDiscontentment(float newValue)
    {
        return newValue * newValue;
    }

    public virtual float GetChange()
    {
        return 0f;
    }
}
