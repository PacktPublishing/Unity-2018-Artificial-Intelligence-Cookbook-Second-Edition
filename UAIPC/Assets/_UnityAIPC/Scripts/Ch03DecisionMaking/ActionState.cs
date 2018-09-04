using UnityEngine;
using System.Collections;

public class ActionState : DecisionTreeNode
{
    public State state;

    public override DecisionTreeNode MakeDecision()
    {
        return this;
    }
}
