using UnityEngine;

public class ConditionTargetFar : Condition
{
    public GameObject origin;
    public GameObject target;
    public float maxDistance;

    public override bool Test()
    {
        if (origin == null || target == null)
            return false;
        Vector3 originPos = origin.transform.position;
        Vector3 targetPos = target.transform.position;
        if (Vector3.Distance(originPos, targetPos) > maxDistance)
            return true;
        return false;
    }
}
