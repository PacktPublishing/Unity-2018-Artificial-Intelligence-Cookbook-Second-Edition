using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentAwared : MonoBehaviour
{
    protected Interest interest;

    public bool IsRelevant(Interest i)
    {
        int oldValue = (int)interest.priority;
        int newValue = (int)i.priority;
        if (newValue <= oldValue)
            return false;
        return true;
    }

    public void Notice(Interest i)
    {
        StopCoroutine(Investigate());
        interest = i;
        StartCoroutine(Investigate());
    }

    public virtual IEnumerator Investigate()
    {
        // TODO
        // develop your implementation
        yield break;
    }

    public virtual IEnumerator Lead()
    {
        // TODO
        // develop your implementation
        yield break;
    }
}
