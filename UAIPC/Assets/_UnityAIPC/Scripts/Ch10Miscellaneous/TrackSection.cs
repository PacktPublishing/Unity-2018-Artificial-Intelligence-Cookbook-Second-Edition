using UnityEngine;
using System.Collections;

public class TrackSection : MonoBehaviour
{
    public bool initialSection;
    public GameObject backL;
    public GameObject backR;
    public GameObject frntL;
    public GameObject frntR;
    public TrackSection next;

    private GameObject backCentroid;
    private GameObject frntCentroid;

    private float length;

    public float Length
    {
        get { return length; }
    }

    void Awake()
    {
        Vector3 centroid = backL.transform.position;
        centroid += backR.transform.position;
        centroid /= 2f;
        backCentroid = new GameObject();
        backCentroid.transform.position = centroid;
        centroid = frntL.transform.position;
        centroid += frntR.transform.position;
        centroid /= 2f;
        frntCentroid = new GameObject();
        frntCentroid.transform.position = centroid;
        length = Vector3.Distance(backCentroid.transform.position, centroid);
    }
}
