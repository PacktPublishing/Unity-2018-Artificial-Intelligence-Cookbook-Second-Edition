using UnityEngine;
using System.Collections;

public class HarmonySearch : MonoBehaviour
{
    [Range(1, 100)]
    public int memorySize = 30;
    public int pitchNum;
    // consolidation rate
    [Range(0.1f, 0.99f)]
    public float consRate = 0.9f;
    // adjustment rate
    [Range(0.1f, 0.99f)]
    public float adjsRate = 0.7f;
    public float range = 0.05f;
    public int numIterations;
    [Range(0.1f, 1.0f)]
    public float par = 0.3f;

    public Vector2[] bounds;

    private float[,] memory;
    private float[] solution;

    // Use this for initialization
    void Start()
    {
    }


    public void Init()
    {
        memory = new float[memorySize, pitchNum];
        solution = new float[memorySize];
    }

    public float[] Run()
    {
        return solution;
    }

    private float GetRandInBounds(Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }

    
}
