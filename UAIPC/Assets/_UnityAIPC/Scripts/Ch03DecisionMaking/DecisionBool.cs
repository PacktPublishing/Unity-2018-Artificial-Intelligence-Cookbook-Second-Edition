using UnityEngine;
using System.Collections;

public class DecisionBool : Decision
{

    public bool valueDecision;
    public bool valueTest;

    public override DTAction GetBranch()
    {
        if (valueTest == valueDecision)
            return nodeTrue;
        return nodeFalse;
    }
}
