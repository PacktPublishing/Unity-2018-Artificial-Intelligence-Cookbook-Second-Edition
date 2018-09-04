using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VertexInfluence : Vertex
{
    public Faction faction;
    public float value = 0f;

    // map flooding
    public Guild guild;

    public bool SetValue(Faction f, float v)
    {
        bool isUpdated = false;
        if (v > value)
        {
            value = v;
            faction = f;
            isUpdated = true;
        }
        return isUpdated;
    }
}
