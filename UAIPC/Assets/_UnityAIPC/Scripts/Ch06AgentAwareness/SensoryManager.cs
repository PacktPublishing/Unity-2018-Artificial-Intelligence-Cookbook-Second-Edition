using UnityEngine;
using System.Collections.Generic;

public class SensoryManager : MonoBehaviour
{

    public List<AgentAwared> agents;
    public List<InterestSource> sources;

    public void Awake()
    {
        agents = new List<AgentAwared>();
        sources = new List<InterestSource>();
    }

    public void UpdateLoop()
    {
        List<AgentAwared> affected;
        AgentAwared leader;
        List<AgentAwared> scouts;
        foreach (InterestSource source in sources)
        {
            if (!source.active)
                continue;
            source.active = false;
            affected = source.GetAffected(agents.ToArray());
            if (affected.Count == 0)
                continue;
            
            int l = Random.Range(0, affected.Count);
            leader = affected[l];
            scouts = GetScouts(affected.ToArray(), l);
            if (leader.Equals(scouts[0]))
                StartCoroutine(leader.Lead());
            foreach (AgentAwared a in scouts)
            {
                Interest i = source.interest;
                if (a.IsRelevant(i))
                    a.Notice(i);
            }
        }
    }

    public List<AgentAwared> GetScouts(AgentAwared[] agents, int leader = -1)
    {
        if (agents.Length == 0)
            return new List<AgentAwared>();
        if (agents.Length == 1)
            return new List<AgentAwared>(agents);

        List<AgentAwared> agentList;
        agentList = new List<AgentAwared>(agents);
        if (leader > -1)
            agentList.RemoveAt(leader);
        List<AgentAwared> scouts;
        scouts = new List<AgentAwared>();
        float numAgents = (float)agents.Length;
        int numScouts = (int)Mathf.Log(numAgents, 2f);
        while (numScouts != 0)
        {
            int numA = agentList.Count;
            int r = Random.Range(0, numA);
            AgentAwared a = agentList[r];
            scouts.Add(a);
            agentList.RemoveAt(r);
            numScouts--;
        }
        return scouts;
    }
    
}
