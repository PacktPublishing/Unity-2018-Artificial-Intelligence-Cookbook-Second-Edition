using UnityEngine;
using System.Collections;

public class VelocityMatch : AgentBehavior
{
    public float timeToTarget = 0.1f;

    public override void Awake()
    {
        base.Awake();
    }

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        steering.linear = target.GetComponent<Agent>().velocity - agent.velocity;

        steering.linear /= timeToTarget;

        if (steering.linear.magnitude > agent.maxAccel)
        {
            steering.linear = steering.linear.normalized * agent.maxAccel;
        }

        steering.angular = 0f;

        return steering;
    }

}