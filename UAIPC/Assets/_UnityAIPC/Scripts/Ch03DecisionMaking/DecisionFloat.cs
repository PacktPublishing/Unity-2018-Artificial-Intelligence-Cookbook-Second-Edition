using UnityEngine;
using System.Collections;

public class DecisionFloat : Decision
{
    public float valueMin;
    public float valueMax;
    public float valueTest;

    public override DTAction GetBranch()
    {
        if (valueMax >= valueTest && valueTest >= valueMin)
            return nodeTrue;
        return nodeFalse;
    }
}
