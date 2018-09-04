using UnityEngine;
using System.Collections;

public class TransitionDecision : Transition
{
    public DecisionTreeNode root;

    public State GetState()
    {
        ActionState action;
        action = root.MakeDecision() as ActionState;
        return action.state;
    }
}
