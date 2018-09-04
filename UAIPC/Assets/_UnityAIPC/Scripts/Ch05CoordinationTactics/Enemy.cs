using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public StageManager stageManager;
    public int slotWeight;
    [HideInInspector]
    public int circleId = -1;
    [HideInInspector]
    public bool isAssigned;
    [HideInInspector]
    public Attack[] attackList;

    void Start()
    {
        attackList = gameObject.GetComponents<Attack>();
    }

    public void SetCircle(GameObject circleObj = null)
    {
        int id = -1;
        if (circleObj == null)
        {
            Vector3 position = transform.position;
            id = stageManager.GetClosestCircle(position);
        }
        else
        {
            FightingCircle fc;
            fc = circleObj.GetComponent<FightingCircle>();
            if (fc != null)
                id = fc.gameObject.GetInstanceID();
        }
        circleId = id;
    }

    public bool RequestSlot()
    {
        isAssigned = stageManager.GrantSlot(circleId, this);
        return isAssigned;
    }

    public void ReleaseSlot()
    {
        stageManager.ReleaseSlot(circleId, this);
        isAssigned = false;
        circleId = -1;
    }

    public bool RequestAttack(int id)
    {
        return stageManager.GrantAttack(circleId, attackList[id]);
    }

    public virtual IEnumerator Attack()
    {
        yield break;
    }

    
}
