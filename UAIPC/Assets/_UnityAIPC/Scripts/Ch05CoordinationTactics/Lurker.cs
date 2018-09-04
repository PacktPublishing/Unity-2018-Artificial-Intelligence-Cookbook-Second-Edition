using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lurker : MonoBehaviour
{
    [HideInInspector]
    public List<Vertex> path;

    void Awake()
    {
        if (ReferenceEquals(path, null))
            path = new List<Vertex>();
    }
}
