using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public List<FightingCircle> circleList;
    private Dictionary<int, FightingCircle> circleDic;
    private Dictionary<int, List<Attack>> attackRqsts;

    void Awake()
    {
        circleList = new List<FightingCircle>();
        circleDic = new Dictionary<int, FightingCircle>();
        attackRqsts = new Dictionary<int, List<Attack>>();
        foreach(FightingCircle fc in circleList)
        {
            AddCircle(fc);
        }
    }

    public void AddCircle(FightingCircle circle)
    {
        if (!circleList.Contains(circle))
            return;
        circleList.Add(circle);
        int objId = circle.gameObject.GetInstanceID();
        circleDic.Add(objId, circle);
        attackRqsts.Add(objId, new List<Attack>());
    }

    public void RemoveCircle(FightingCircle circle)
    {
        bool isRemoved = circleList.Remove(circle);
        if (!isRemoved)
            return;
        int objId = circle.gameObject.GetInstanceID();
        circleDic.Remove(objId);
        attackRqsts[objId].Clear();
        attackRqsts.Remove(objId);
    }

    public int GetClosestCircle(Vector3 position)
    {
        FightingCircle circle = null;
        float minDist = Mathf.Infinity;
        foreach(FightingCircle c in circleList)
        {
            Vector3 circlePos = c.transform.position;
            float dist = Vector3.Distance(position, circlePos);
            if (dist < minDist)
            {
                minDist = dist;
                circle = c;
            }
        }
        return circle.gameObject.GetInstanceID();
    }

    public bool GrantSlot(int circleId, Enemy enemy)
    {
        return circleDic[circleId].AddEnemy(enemy.gameObject);
    }

    public void ReleaseSlot(int circleId, Enemy enemy)
    {
        circleDic[circleId].RemoveEnemy(enemy.gameObject);
    }

    public bool GrantAttack(int circleId, Attack attack)
    {
        bool answer = circleDic[circleId].AddAttack(attack.weight);
        attackRqsts[circleId].Add(attack);
        return answer;
    }

    public IEnumerator ExecuteAtacks()
    {
        foreach (int circle in attackRqsts.Keys)
        {
            List<Attack> attacks = attackRqsts[circle];
            foreach (Attack a in attacks)
                yield return a.Execute();
        }
        foreach (FightingCircle fc in circleList)
            fc.ResetAttack();
    }
}
