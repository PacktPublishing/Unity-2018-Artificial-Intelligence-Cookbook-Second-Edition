using UnityEngine;
using System.Collections;

public class DecisionTreeNode : MonoBehaviour
{

    public virtual DecisionTreeNode MakeDecision()
    {
        return null;
    }

}
