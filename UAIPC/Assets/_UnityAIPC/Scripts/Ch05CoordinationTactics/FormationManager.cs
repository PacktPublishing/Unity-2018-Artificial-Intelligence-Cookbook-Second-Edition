using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormationManager : MonoBehaviour
{
    public FormationPattern pattern;
    private List<SlotAssignment> slotAssignments;
    private Location driftOffset;

    void Awake()
    {
        slotAssignments = new List<SlotAssignment>();
    }

    public void UpdateSlotAssignments()
    {
        for (int i = 0; i < slotAssignments.Count; i++)
        {
            slotAssignments[i].slotIndex = i;
        }
        driftOffset = pattern.GetDriftOffset(slotAssignments);
    }

    public bool AddCharacter(GameObject character)
    {
        int occupiedSlots = slotAssignments.Count;
        if (!pattern.SupportsSlots(occupiedSlots + 1))
            return false;
        SlotAssignment sa = new SlotAssignment();
        sa.character = character;
        slotAssignments.Add(sa);
        UpdateSlotAssignments();
        return true;
    }

    public void RemoveCharacter(GameObject agent)
    {
        int index = slotAssignments.FindIndex(x => x.character.Equals(agent));
        slotAssignments.RemoveAt(index);
        UpdateSlotAssignments();
    }

    public void UpdateSlots()
    {
        GameObject leader = pattern.leader;
        Vector3 anchor = leader.transform.position;
        Vector3 slotPos;
        Quaternion rotation;
        rotation = leader.transform.rotation;
        foreach (SlotAssignment sa in slotAssignments)
        {
            Vector3 relPos;            
            slotPos = pattern.GetSlotLocation(sa.slotIndex);
            relPos = anchor;
            relPos += leader.transform.TransformDirection(slotPos);
            Location charDrift = new Location(relPos, rotation);
            charDrift.position += driftOffset.position;
            charDrift.rotation *= driftOffset.rotation;
            Character character = sa.character.GetComponent<Character>();
            character.SetTarget(charDrift);
        }
    }
}
