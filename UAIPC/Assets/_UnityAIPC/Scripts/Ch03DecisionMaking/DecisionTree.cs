using UnityEngine;
using System.Collections;

public class DecisionTree : DecisionTreeNode
{
    public DecisionTreeNode root;
    private DTAction actionNew;
    private DTAction actionOld;

    void Update()
    {
        actionNew.activated = false;        
        actionOld = actionNew;
        actionNew = root.MakeDecision() as DTAction;
        if (actionNew == null)
            actionNew = actionOld;
        actionNew.activated = true;
    }

    public override DecisionTreeNode MakeDecision()
    {
        return root.MakeDecision();
    }
}
