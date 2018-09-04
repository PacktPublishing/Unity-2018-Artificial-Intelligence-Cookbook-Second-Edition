using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormationPattern: MonoBehaviour
{
    public int numOfSlots;
    public GameObject leader;

    void Start()
    {
        if (leader == null)
            leader = transform.gameObject;
    }

    public virtual Vector3 GetSlotLocation(int slotIndex)
    {
        return Vector3.zero;
    }

    public bool SupportsSlots(int slotCount)
    {
        return slotCount <= numOfSlots;
    }

    public virtual Location GetDriftOffset(List<SlotAssignment> slotAssignments)
    {
        Location location = new Location();
        location.position = leader.transform.position;
        location.rotation = leader.transform.rotation;
        return location;
    }
}
