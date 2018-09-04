using UnityEngine;
using System.Collections.Generic;

public class Track : MonoBehaviour
{

    LinkedList<TrackSection> sections;

    void Awake()
    {
        TrackSection[] trackSections;
        trackSections = FindObjectsOfType<TrackSection>();
        TrackSection section = null;
        foreach (TrackSection ts in trackSections)
        {
            if (ts.initialSection)
            {
                section = ts;
                break;
            }
        }
        if (ReferenceEquals(section, null))
            return;

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
