using UnityEngine;
using System.Collections.Generic;

public class Dungeon : MonoBehaviour
{
  public int dungeonWidth;
  public int dungeonHeight;
  public float minAreaForRoom;
  [Range(0f, 1f)]
  public float middleThreshold;
  public GameObject floorPrefab;

  private List<BSPNode> tree;
  private List<BSPNode> generated;
  private List<BSPNode> expanded;

  void Awake()
  {
    tree = new List<BSPNode>();
    generated = new List<BSPNode>();
    expanded = new List<BSPNode>();
  }

  public void Generate()
  {
    BuildTree();
  }

  private void BuildTree()
  {
    generated.Clear();
    tree.Clear();
    Rect r = new Rect();
    r.width = dungeonWidth;
    r.height = dungeonHeight;
    r.x = -(r.width / 2);
    r.y = -(r.height / 2);
    BSPNode n = new BSPNode(r);
    generated.Add(n);
    float area = GetAvgArea(generated.ToArray());
    while (area >= minAreaForRoom)
    {
      expanded = new List<BSPNode>(generated);
      generated.Clear();
      int i;
      for (i = 0; i < expanded.Count; i++)
      {
        tree.Add(expanded[i]);
        BSPNode nodeA = null;
        BSPNode nodeB = null;
        Split(expanded[i], ref nodeA, ref nodeB);
        generated.Add(nodeA);
        generated.Add(nodeB);
      }
      area = GetAvgArea(generated.ToArray());
    }
  }

  // TODO
  // function for building the tree in the end

  private void Split (BSPNode node, ref BSPNode nA, ref BSPNode nB)
  {
    Rect rectA = new Rect();
    Rect rectB = new Rect();
    float middle;
    int rand = Random.Range(0, 1);
    if (rand == 0)
    {
      // split horizontal
      
    }
    else
    {
      // split vertical
    }
  }

  // TODO
  // improvement split works as BFS
  public void Split()
  {
    float x, y, w, h;
    x = dungeonWidth / 2f * -1f;
    y = dungeonHeight / 2f * -1f;
    w = dungeonWidth;
    h = dungeonHeight;
    Rect rootRect = new Rect(x, y, w, h);
    
  }

  public float GetAvgArea(BSPNode[] nodes)
  {
    float area = 0f;
    if (nodes.Length == 0)
      return area;
    foreach (BSPNode n in nodes)
    {
      area += n.rect.width * n.rect.height;
    }
    area /= nodes.Length;
    return area;
  }

  public void DrawNode(BSPNode n)
  {
    GameObject go = Instantiate(floorPrefab) as GameObject;
    Vector3 position = new Vector3(n.rect.x, 0f, n.rect.y);
    Vector3 scale = new Vector3(n.rect.width, 1f, n.rect.height);
    go.transform.position = position;
    go.transform.localScale = scale;
  }
}
