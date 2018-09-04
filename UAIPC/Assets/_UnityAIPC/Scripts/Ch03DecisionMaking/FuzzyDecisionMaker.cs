using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuzzyDecisionMaker : MonoBehaviour
{
    public Dictionary<int,float> MakeDecision(object[] inputs, MembershipFunction[][] mfList, FuzzyRule[] rules)
    {
        Dictionary<int, float> inputDOM = new Dictionary<int, float>();
        Dictionary<int, float> outputDOM = new Dictionary<int, float>();
        MembershipFunction memberFunc;

        foreach (object input in inputs)
        {
            int r, c;
            for (r = 0; r < mfList.Length; r++)
            {
                for (c = 0; c < mfList[r].Length; c++)
                {
                    memberFunc = mfList[r][c];
                    int mfId = memberFunc.stateId;
                    float dom = memberFunc.GetDOM(input);
                    if (!inputDOM.ContainsKey(mfId))
                    {
                        inputDOM.Add(mfId, dom);
                        outputDOM.Add(mfId, 0f);
                    }
                    else
                        inputDOM[mfId] = dom;
                }
            }
        }

        foreach (FuzzyRule rule in rules)
        {
            int outputId = rule.conclusionStateId;
            float best = outputDOM[outputId];
            float min = 1f;
            foreach (int state in rule.stateIds)
            {
                float dom = inputDOM[state];
                if (dom < best)
                    continue;
                if (dom < min)
                    min = dom;
            }
            outputDOM[outputId] = min;
        }
        return outputDOM;
    }
}
