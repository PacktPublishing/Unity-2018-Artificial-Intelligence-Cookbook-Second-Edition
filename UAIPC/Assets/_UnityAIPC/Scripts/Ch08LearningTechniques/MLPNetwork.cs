using UnityEngine;
using System.Collections;

public class MLPNetwork : MonoBehaviour
{

    public Perceptron[] inputPer;
    public Perceptron[] hiddenPer;
    public Perceptron[] outputPer;

    public void GenerateOutput(Perceptron[] inputs)
    {
        int i;
        for (i = 0; i < inputs.Length; i++)
            inputPer[i].state = inputs[i].input;
        
        for (i = 0; i < hiddenPer.Length; i++)
            hiddenPer[i].FeedForward();
        
        for (i = 0; i < outputPer.Length; i++)
            outputPer[i].FeedForward();
    }

    public void BackProp(Perceptron[] outputs)
    {
        int i;
        for (i = 0; i < outputPer.Length; i++)
        {
            Perceptron p = outputPer[i];
            float state = p.state;

            float error = state * (1f - state);
            error *= outputs[i].state - state;

            p.AdjustWeights(error);
        }

        for (i = 0; i < hiddenPer.Length; i++)
        {
            Perceptron p = outputPer[i];
            float state = p.state;

            float sum = 0f;

            for (i = 0; i < outputs.Length; i++)
            {
                float incomingW = outputs[i].GetIncomingWeight();
                sum += incomingW * outputs[i].error;

                float error = state * (1f - state) * sum;

                p.AdjustWeights(error);
            }
        }
    }

    public void Learn(
            Perceptron[] inputs,
            Perceptron[] outputs)
    {
        GenerateOutput(inputs);
        BackProp(outputs);
    }

    
}
