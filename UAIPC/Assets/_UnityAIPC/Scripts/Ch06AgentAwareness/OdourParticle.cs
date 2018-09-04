using UnityEngine;
using System.Collections;

public class OdourParticle : MonoBehaviour
{
    public float timespan;
    private float timer;
    
    public int parent;

    void Start()
    {
        if (timespan < 0f)
            timespan = 0f;
        timer = timespan;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
            Destroy(gameObject);
    }
}
