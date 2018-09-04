using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TFRAxisCompare
{
    X, Y, Z
}

public enum TFRState
{
    ATTACK, DEFEND, OPEN
}

public class TFRival : MonoBehaviour
{

    public string tagPiece = "TFPiece";
    public string tagWall = "TFWall";
    public int numBarsToHandle = 2;
    public float handleSpeed;
    public float attackDistance;
    public TFRAxisCompare depthAxis = TFRAxisCompare.Z;
    public TFRAxisCompare widthAxis = TFRAxisCompare.X;
    public GameObject ball;
    public GameObject[] bars;

    List<GameObject>[] pieceList;


    void Awake()
    {
        int numBars = bars.Length;
        pieceList = new List<GameObject>[numBars];
        for (int i = 0; i < numBars; i++)
        {
            pieceList[i] = new List<GameObject>();
        }
    }

    void Start()
    {

    }


    void Update()
    {
        int[] currBars = GetNearestBars();
        Vector3 ballPos = ball.transform.position;
        Vector3 barsPos;
        int i;
        for (i = 0; i < currBars.Length; i++)
        {
            GameObject barObj = bars[currBars[i]];
            TFRBar bar = barObj.GetComponent<TFRBar>();
            barsPos = barObj.transform.position;
            float ballVisible = Vector3.Dot(barsPos, ballPos);
            float dist = Vector3.Distance(barsPos, ballPos);
            if (ballVisible > 0f && dist <= attackDistance)
                bar.SetState(TFRState.ATTACK, handleSpeed);
            else if (ballVisible > 0f)
                bar.SetState(TFRState.DEFEND);
            else
                bar.SetState(TFRState.OPEN);
        }
    }

    public void OnGUI()
    {
        Predict();
    }

    private void Predict()
    {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        Vector3 position = rb.position;
        Vector3 velocity = rb.velocity.normalized;
        int[] barsToCheck = GetNearestBars();
        List<GameObject> barsChecked;
        GameObject piece;
        barsChecked = new List<GameObject>();
        int id = -1;
        do
        {
            RaycastHit[] hits = Physics.RaycastAll(position, velocity.normalized);
            RaycastHit wallHit;
            foreach (RaycastHit h in hits)
            {
                GameObject obj = h.collider.gameObject;
                if (obj.CompareTag(tagWall))
                    wallHit = h;
                if (!IsBar(obj))
                    continue;
                if (barsChecked.Contains(obj))
                    continue;
                bool isToCheck = false;
                
                for (int i = 0; i < barsToCheck.Length; i++)
                {
                    id = barsToCheck[i];
                    GameObject barObj = bars[id];
                    if (obj == barObj)
                    {
                        isToCheck = true;
                        break;
                    }         
                }
                if (!isToCheck)
                    continue;
                Vector3 p = h.point;
                piece = GetNearestPiece(h.point, id);
                Vector3 piecePos = piece.transform.position;
                float diff = Vector3.Distance(h.point, piecePos);
                obj.GetComponent<TFRBar>().Slide(diff, handleSpeed);
                barsChecked.Add(obj);
            }

        } while (barsChecked.Count == numBarsToHandle);
    }

    void SetPieces()
    {
        // Create a dictionary between z-index and bar
        Dictionary<float, int> zBarDict;
        zBarDict = new Dictionary<float, int>();
        int i;
        for (i = 0; i < bars.Length; i++)
        {
            Vector3 p = bars[i].transform.position;
            float index = GetVectorAxis(p, this.depthAxis);
            zBarDict.Add(index, i);
        }
        // Map the pieces to the bars
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tagPiece);
        Dictionary<float, List<GameObject>> dict;
        dict = new Dictionary<float, List<GameObject>>();
        foreach (GameObject p in objs)
        {
            float zIndex = p.transform.position.z;
            if (!dict.ContainsKey(zIndex))
                dict.Add(zIndex, new List<GameObject>());
            dict[zIndex].Add(p);
        }
    }

    int GetBarIndex(Vector3 position, TFRAxisCompare axis)
    {
        int index = 0;
        if (bars.Length == 0)
            return index;

        float pos = GetVectorAxis(position, axis);
        float min = Mathf.Infinity;
        float barPos;
        Vector3 p;
        for (int i = 0; i < bars.Length; i++)
        {
            p = bars[i].transform.position;
            barPos = GetVectorAxis(p, axis);
            float diff = Mathf.Abs(pos - barPos);
            if (diff < min)
            {
                min = diff;
                index = i;
            }
        }
        return index;
    }

    float GetVectorAxis(Vector3 v, TFRAxisCompare a)
    {
        if (a == TFRAxisCompare.X)
            return v.x;
        if (a == TFRAxisCompare.Y)
            return v.y;
        return v.z;
    }

    public int[] GetNearestBars()
    {
        int numBars = Mathf.Clamp(numBarsToHandle, 0, bars.Length);
        Dictionary<float, int> distBar;
        distBar = new Dictionary<float, int>(bars.Length);
        List<float> distances = new List<float>(bars.Length);
        int i;
        Vector3 ballPos = ball.transform.position;
        Vector3 barPos;
        for (i = 0; i < bars.Length; i++)
        {
            barPos = bars[i].transform.position;
            float d = Vector3.Distance(ballPos, barPos);
            distBar.Add(d, i);
            distances.Add(d);
        }
        distances.Sort();
        int[] barsNear = new int[numBars];
        for (i = 0; i < numBars; i++)
        {
            float d = distances[i];
            int id = distBar[d];
            barsNear[i] = id;
        }
        return barsNear;
    }

    private bool IsBar(GameObject gobj)
    {
        foreach (GameObject b in bars)
        {
            if (b == gobj)
                return true;
        }
        return false;
    }

    private GameObject GetNearestPiece(Vector3 position, int barId)
    {
        float minDist = Mathf.Infinity;
        float dist;
        GameObject piece = null;
        foreach (GameObject p in pieceList[barId])
        {
            dist = Vector3.Distance(position, p.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                piece = p;
            }
        }
        return piece;
    }
}
