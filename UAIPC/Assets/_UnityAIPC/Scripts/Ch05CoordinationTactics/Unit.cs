using UnityEngine;
using System.Collections;

public enum Faction
{
    BLUE, RED
}

public class Unit : MonoBehaviour
{
    public Faction faction;
    public int radius = 1;
    public float influence = 1f;

    public virtual float GetDropOff(int locationDistance)
    {
        // simple
        //return influence;
        // more elaborate
        float d = influence / radius * locationDistance;
        return influence - d;
    }
}
