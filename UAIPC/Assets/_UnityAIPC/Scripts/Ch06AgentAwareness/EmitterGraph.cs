//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class EmitterGraph : MonoBehaviour
//{
//    public int soundIntensity;
//    public Graph soundGraph;
//    public GameObject emitterObj;

//    public void Start()
//    {
//        if (emitterObj == null)
//            emitterObj = gameObject;
//    }

//    public int[] Emit()
//    {
//        List<int> nodeIds = new List<int>();
//        Queue<int> queue = new Queue<int>();
//        List<int> neighbours;
//        int size = soundGraph.GetSize();
//        bool[] visited = new bool[size];
//        int intensity = soundIntensity;
//        int src = soundGraph.GetNearestVertex(emitterObj);
//        nodeIds.Add(src);
//        queue.Enqueue(src);
//        while (queue.Count != 0)
//        {
//            if (intensity == 0)
//                break;
//            int v = queue.Dequeue();
//            neighbours = soundGraph.GetNeighbours(v);
//            foreach (int n in neighbours)
//            {
//                if (visited[n])
//                    continue;
//                queue.Enqueue(n);
//                nodeIds.Add(n);
//            }
//            intensity--;
//        }
//        return nodeIds.ToArray();
//    }
//}
