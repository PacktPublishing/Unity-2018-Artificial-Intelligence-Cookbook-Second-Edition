using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightingCircle : MonoBehaviour
{
    public int slotCapacity;
    public int attackCapacity;
    public float attackRadius;
    public GameObject player;

    [HideInInspector]
    public int slotsAvailable;
    [HideInInspector]
    public int attackAvailable;
    [HideInInspector]
    public List<GameObject> enemyList;
    [HideInInspector]
    public Dictionary<int, Vector3> posDict;

    void Awake()
    {
        slotsAvailable = slotCapacity;
        attackAvailable = attackCapacity;
        enemyList = new List<GameObject>();
        posDict = new Dictionary<int, Vector3>();
        if (player == null)
            player = gameObject;
    }

    void Update()
    {
        if (enemyList.Count == 0)
            return;
        Vector3 anchor = player.transform.position;
        int i;
        for (i = 0; i < enemyList.Count; i++)
        {
            Vector3 position = anchor;
            Vector3 slotPos = GetSlotLocation(i);
            int enemyId = enemyList[i].GetInstanceID();
            position += player.transform.TransformDirection(slotPos);
            posDict[enemyId] = position;
        }
    }

    public bool AddEnemy(GameObject enemyObj)
    {
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        int enemyId = enemyObj.GetInstanceID();
        if (slotsAvailable < enemy.slotWeight)
            return false;
        enemyList.Add(enemyObj);
        posDict.Add(enemyId, Vector3.zero);
        slotsAvailable -= enemy.slotWeight;
        return true;
    }

    public bool RemoveEnemy(GameObject enemyObj)
    {
        bool isRemoved = enemyList.Remove(enemyObj);
        if (isRemoved)
        {
            int enemyId = enemyObj.GetInstanceID();
            posDict.Remove(enemyId);
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            slotsAvailable += enemy.slotWeight;
        }
        return isRemoved;
    }

    public void SwapEnemies(GameObject enemyObjA, GameObject enemyObjB)
    {
        int indexA = enemyList.IndexOf(enemyObjA);
        int indexB = enemyList.IndexOf(enemyObjB);
        if (indexA != -1 && indexB != -1)
        {
            enemyList[indexB] = enemyObjA;
            enemyList[indexA] = enemyObjB;
        }
    }

    public Vector3? GetPositions(GameObject enemyObj)
    {
        int enemyId = enemyObj.GetInstanceID();
        if (!posDict.ContainsKey(enemyId))
            return null;
        return posDict[enemyId];
    }

    private Vector3 GetSlotLocation(int slot)
    {
        Vector3 location = new Vector3();
        float degrees = 360f / enemyList.Count;
        degrees *= (float)slot;
        location.x = Mathf.Cos(Mathf.Deg2Rad * degrees);
        location.x *= attackRadius;
        location.z = Mathf.Cos(Mathf.Deg2Rad * degrees);
        location.z *= attackRadius;
        return location;
    }

    public bool AddAttack(int weight)
    {
        if (attackAvailable - weight < 0)
            return false;
        attackAvailable -= weight;
        return true;
    }

    public void ResetAttack()
    {
        attackAvailable = attackCapacity;
    }
}
