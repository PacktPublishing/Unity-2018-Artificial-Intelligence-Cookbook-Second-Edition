using UnityEngine;
using System;
using System.Collections;

public class Guild : MonoBehaviour
{
    public string guildName;
    public int maxStrength;
    public GameObject baseObject;
    [HideInInspector]
    public int strenght;

    public virtual void Awake()
    {
        strenght = maxStrength;
    }
    
    public virtual float GetDropOff(float distance)
    {
        // simple
        //return strength;
        // more elaborate
        float d = Mathf.Pow(1 + distance, 2f);
        return strenght / d;
    }
}
