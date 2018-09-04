using UnityEngine;
using System.Collections.Generic;

public class ID3 : MonoBehaviour
{

    public float GetEntropy(ID3Example[] examples)
    {
        if (examples.Length == 0) return 0f;
        int numExamples = examples.Length;
        Dictionary<ID3Action, int> actionTallies;
        actionTallies = new Dictionary<ID3Action, int>();
        foreach (ID3Example e in examples)
        {
            if (!actionTallies.ContainsKey(e.action))
                actionTallies.Add(e.action, 0);
            actionTallies[e.action]++;
        }

        int actionCount = actionTallies.Keys.Count;
        if (actionCount == 0) return 0f;
        float entropy = 0f;
        float proportion = 0f;
        foreach (int tally in actionTallies.Values)
        {
            proportion = tally / (float)numExamples;
            entropy -= proportion * Mathf.Log(proportion, 2);
        }
        return entropy;
    }

    public Dictionary<float, List<ID3Example>> SplitByAttribute(
            ID3Example[] examples,
            string attribute)
    {
        Dictionary<float, List<ID3Example>> sets;
        sets = new Dictionary<float, List<ID3Example>>();
        foreach (ID3Example e in examples)
        {
            float key = e.GetValue(attribute);
            if (!sets.ContainsKey(key))
                sets.Add(key, new List<ID3Example>());
            sets[key].Add(e);
        }
        return sets;
    }

    public float GetEntropy(
            Dictionary<float, List<ID3Example>> sets,
            int numExamples)
    {
        float entropy = 0f;
        foreach (List<ID3Example> s in sets.Values)
        {
            float proportion;
            proportion = s.Count / (float)numExamples;
            entropy -= proportion * GetEntropy(s.ToArray());
        }
        return entropy;
    }

    public void MakeTree(
            ID3Example[] examples,
            List<string> attributes,
            DecisionNode node)
    {
        float initEntropy = GetEntropy(examples);
        if (initEntropy <= 0) return;

        int numExamples = examples.Length;
        float bestInfoGain = 0f;
        string bestSplitAttribute = "";
        float infoGain = 0f;
        float overallEntropy = 0f;
        Dictionary<float, List<ID3Example>> bestSets;
        bestSets = new Dictionary<float, List<ID3Example>>();
        Dictionary<float, List<ID3Example>> sets;
        foreach (string a in attributes)
        {
            sets = SplitByAttribute(examples, a);
            overallEntropy = GetEntropy(sets, numExamples);
            infoGain = initEntropy - overallEntropy;
            if (infoGain > bestInfoGain)
            {
                bestInfoGain = infoGain;
                bestSplitAttribute = a;
                bestSets = sets;
            }
        }

        node.testValue = bestSplitAttribute;
        List<string> newAttributes = new List<string>(attributes);
        newAttributes.Remove(bestSplitAttribute);

        foreach (List<ID3Example> set in bestSets.Values)
        {
            float val = set[0].GetValue(bestSplitAttribute);
            DecisionNode child = new DecisionNode();
            node.children.Add(val, child);
            MakeTree(set.ToArray(), newAttributes, child);
        }
    }
}
