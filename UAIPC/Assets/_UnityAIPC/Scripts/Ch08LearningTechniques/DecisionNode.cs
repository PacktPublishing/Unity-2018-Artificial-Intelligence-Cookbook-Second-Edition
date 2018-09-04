using System.Collections.Generic;

public class DecisionNode
{
    public string testValue;
    public Dictionary<float, DecisionNode> children;

    public DecisionNode(string testValue = "")
    {
        this.testValue = testValue;
        children = new Dictionary<float, DecisionNode>();
    }
}
