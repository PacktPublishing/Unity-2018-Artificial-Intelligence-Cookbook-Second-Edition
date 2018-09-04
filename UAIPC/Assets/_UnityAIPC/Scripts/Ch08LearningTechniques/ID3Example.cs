using UnityEngine;
using System.Collections.Generic;

public enum ID3Action
{
    STOP, WALK, RUN
}

public class ID3Example : MonoBehaviour
{
    public ID3Action action;
    public Dictionary<string, float> values;
    
    public float GetValue(string attribute)
    {
        return values[attribute];
    }
}
