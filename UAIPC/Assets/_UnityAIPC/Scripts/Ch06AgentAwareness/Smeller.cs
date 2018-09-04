using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Smeller : MonoBehaviour
{
    private Vector3 target;
    private Dictionary<int, GameObject> particles;

    void Start()
    {
        particles = new Dictionary<int, GameObject>();
    }

    public void OnTriggerEnter(Collider coll)
    {
        GameObject obj = coll.gameObject;
        OdourParticle op;
        op = obj.GetComponent<OdourParticle>();
        if (op == null)
            return;
        int objId = obj.GetInstanceID();
        particles.Add(objId, obj);
        UpdateTarget();
    }

    public void OnTriggerExit(Collider coll)
    {
        GameObject obj = coll.gameObject;
        int objId = obj.GetInstanceID();
        bool isRemoved;
        isRemoved = particles.Remove(objId);
        if (!isRemoved)
            return;
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        Vector3 centroid = Vector3.zero;
        foreach (GameObject p in particles.Values)
        {
            Vector3 pos = p.transform.position;
            centroid += pos;
        }
        target = centroid;
    }

    public Vector3? GetTargetPosition()
    {
        if (particles.Keys.Count == 0)
            return null;
        return target;
    }


}
