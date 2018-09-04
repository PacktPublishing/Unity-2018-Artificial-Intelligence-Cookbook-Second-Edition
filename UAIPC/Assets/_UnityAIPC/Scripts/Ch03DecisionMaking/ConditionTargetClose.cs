using UnityEngine;

public class ConditionTargetClose : Condition
{
    public GameObject origin;
    public GameObject target;
    public float minDistance;

    public override bool Test()
    {
        if (origin == null || target == null)
            return false;
        Vector3 originPos = origin.transform.position;
        Vector3 targetPos = target.transform.position;
        if (Vector3.Distance(originPos, targetPos) <= minDistance)
            return true;
        return false;
    }
}
