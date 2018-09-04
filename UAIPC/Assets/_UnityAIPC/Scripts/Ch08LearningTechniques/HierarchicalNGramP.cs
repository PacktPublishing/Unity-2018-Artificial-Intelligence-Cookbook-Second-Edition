using System;
using System.Collections;
using System.Text;

public class HierarchicalNGramP<T>
{
    
    public int threshold;
    public NGramPredictor<T>[] predictors;
    private int nValue;


    public HierarchicalNGramP(int windowSize)
    {
        nValue = windowSize + 1;
        predictors = new NGramPredictor<T>[nValue];
        int i;
        for (i = 0; i < nValue; i++)
            predictors[i] = new NGramPredictor<T>(i + 1);
    }

    public void RegisterSequence(T[] actions)
    {
        int i;
        for (i = 0; i < nValue; i++)
        {
            T[] subactions = new T[i+1];
            Array.Copy(actions, nValue - i - 1, subactions, 0, i+1);
            predictors[i].RegisterSequence(subactions);
        }
    }

    public T GetMostLikely(T[] actions)
    {
        int i;
        T bestAction = default(T);
        for (i = 0; i < nValue; i++)
        {
            NGramPredictor<T> p;
            p = predictors[nValue - i - 1];
            T[] subactions = new T[i + 1];
            Array.Copy(actions, nValue - i - 1, subactions, 0, i + 1);
            int numActions = p.GetActionsNum(ref actions);
            if (numActions > threshold)
                bestAction = p.GetMostLikely(actions);
        }
        return bestAction;
    }

    private string ArrToStrKey(ref T[] actions)
    {
        StringBuilder builder = new StringBuilder();
        foreach (T a in actions)
        {
            builder.Append(a.ToString());
        }
        return builder.ToString();
    }
}
