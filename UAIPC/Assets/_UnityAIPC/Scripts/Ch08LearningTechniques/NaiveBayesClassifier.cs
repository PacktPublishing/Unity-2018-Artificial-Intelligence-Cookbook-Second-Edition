using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum NBCLabel
{
    POSITIVE,
    NEGATIVE
}

public class NaiveBayesClassifier : MonoBehaviour
{

    public int numAttributes;
    public int numExamplesPositive;
    public int numExamplesNegative;

    public List<bool> attrCountPositive;
    public List<bool> attrCountNegative;

    void Awake()
    {
        attrCountPositive = new List<bool>();
        attrCountNegative = new List<bool>();
    }

    public void UpdateClassifier(bool[] attributes, NBCLabel label)
    {
        if (label == NBCLabel.POSITIVE)
        {
            numExamplesPositive++;
            attrCountPositive.AddRange(attributes);
        }
        else
        {
            numExamplesNegative++;
            attrCountNegative.AddRange(attributes);
        }
    }

    public bool Predict(bool[] attributes)
    {
        float nep = numExamplesPositive;
        float nen = numExamplesNegative;
        float x = NaiveProbabilities(ref attributes, attrCountPositive.ToArray(), nep, nen);
        float y = NaiveProbabilities(ref attributes, attrCountNegative.ToArray(), nen, nep);
        if (x >= y)
            return true;
        return false;
    }

    public float NaiveProbabilities(
            ref bool[] attributes,
            bool[] counts,
            float m,
            float n)
    {
        float prior = m / (m + n);
        float p = 1f;
        int i = 0;
        for (i = 0; i < numAttributes; i++)
        {
            p /= m;
            if (attributes[i] == true)
                p *= counts[i].GetHashCode();
            else
                p *= m - counts[i].GetHashCode();
        }
        return prior * p;
    }
}
