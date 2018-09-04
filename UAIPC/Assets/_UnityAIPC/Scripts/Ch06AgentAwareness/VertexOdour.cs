using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VertexOdour : Vertex
{
    private Dictionary<int, OdourParticle> odourDic;

    public void Start()
    {
        odourDic = new Dictionary<int, OdourParticle>();
    }

    public void OnCollisionEnter(Collision coll)
    {
        OdourParticle op;
        op = coll.gameObject.GetComponent<OdourParticle>();
        if (op == null)
            return;
        int id = op.parent;
        odourDic.Add(id, op);
    }

    public void OnCollisionExit(Collision coll)
    {
        OdourParticle op;
        op = coll.gameObject.GetComponent<OdourParticle>();
        if (op == null)
            return;
        int id = op.parent;
        odourDic.Remove(id);
    }

    public bool HasOdour()
    {
        if (odourDic.Values.Count == 0)
            return false;
        return true;
    }

    public bool OdourExists(int id)
    {
        return odourDic.ContainsKey(id);
    }

}
