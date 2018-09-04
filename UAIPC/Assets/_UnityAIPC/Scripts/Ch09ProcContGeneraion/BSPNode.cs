using UnityEngine;

[System.Serializable]
public class BSPNode
{
  public Rect rect;
  public BSPNode nodeA;
  public BSPNode nodeB;

  public BSPNode(Rect r, BSPNode nA = null, BSPNode nB = null)
  {
    rect = r;
    nodeA = nA;
    nodeB = nB;
  }
}
