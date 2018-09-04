using UnityEngine;
using System.Collections;

public class Decision : DecisionTreeNode
{
    public DTAction nodeTrue;
    public DTAction nodeFalse;

    public virtual DTAction GetBranch()
    {
        return null;
    }
}
