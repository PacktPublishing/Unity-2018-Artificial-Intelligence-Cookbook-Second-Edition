using UnityEngine;
using System.Collections;

public struct Interest
{
    public InterestSense sense;
    public InterestPriority priority;
    public Vector3 position;

    public Interest(
            InterestSense sense,
            InterestPriority priority,
            Vector3 position)
    {
        this.sense = sense;
        this.priority = priority;
        this.position = position;
    }
}
