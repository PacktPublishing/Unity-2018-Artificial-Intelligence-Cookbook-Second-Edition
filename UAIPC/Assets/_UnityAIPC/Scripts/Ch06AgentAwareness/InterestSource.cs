using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum InterestSense
{
    SOUND,
    SIGHT
};

public enum InterestPriority
{
    LOWEST = 0,
    BROKEN = 1,
    MISSING = 2,
    SUSPECT = 4,
    SMOKE = 4,
    BOX = 5,
    DISTRACTIONFLARE = 10,
    TERROR = 20
};


public class InterestSource : MonoBehaviour
{
    public InterestSense sense;
    public float radius;
    public InterestPriority priority;
    public bool active;

    public Interest interest
    {
        get
        {
            Interest i;
            i.position = transform.position;
            i.priority = priority;
            i.sense = sense;
            return i;
        }
    }


    public virtual List<AgentAwared> GetAffected(AgentAwared[] agentList)
    {
        List<AgentAwared> affected;
        affected = new List<AgentAwared>();
        Vector3 interPos = transform.position;
        Vector3 agentPos;
        float distance;
        foreach (AgentAwared agent in agentList)
        {
            agentPos = agent.transform.position;
            distance = Vector3.Distance(interPos, agentPos);
            if (distance > radius)
                continue;
            bool isAffected = false;
            switch (sense)
            {
                case InterestSense.SIGHT:
                    isAffected = IsAffectedSight(agent);
                    break;
                case InterestSense.SOUND:
                    isAffected = IsAffectedSound(agent);
                    break;
            }

            if (!isAffected)
                continue;
            affected.Add(agent);
        }
        return affected;
    }

    protected bool IsAffectedSight(AgentAwared agent)
    {
        // TODO
        // your sight check implementation
        return false;
    }

    protected bool IsAffectedSound(AgentAwared agent)
    {
        // TODO
        // your sound check implementation
        return false;
    }

}
