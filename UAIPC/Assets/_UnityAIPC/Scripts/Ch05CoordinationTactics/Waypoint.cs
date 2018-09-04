using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour, IComparer
{
    public float value;
    public List<Waypoint> neighbours;

    
    public int Compare(object a, object b)
    {
        Waypoint wa = (Waypoint)a;
        Waypoint wb = (Waypoint)b;
        if (wa.value == wb.value)
            return 0;
        if (wa.value < wb.value)
            return -1;
        return 1;
    }

    public static bool CanMove(Waypoint a, Waypoint b)
    {
        // implement your own behaviour for
        // deciding whether an agent can move
        // easily between two waypoints
        return true;
    }

    public static bool IsInSameRoom(Vector3 from, Vector3 location, string tagWall = "Wall")
    {
        RaycastHit[] hits;
        Vector3 direction = location - from;
        float rayLength = direction.magnitude;
        direction.Normalize();
        Ray ray = new Ray(from, direction);
        hits = Physics.RaycastAll(ray, rayLength);
        foreach (RaycastHit h in hits)
        {
            string tagObj = h.collider.gameObject.tag;
            if (tagObj.Equals(tagWall))
                return false;
        }
        return true;
    }

    public static float GetCoverQuality(
            Vector3 location,
            int iterations,
            Vector3 characterSize,
            float radius,
            float randomRadius,
            float deltaAngle)
    {
        float theta = 0f;
        int hits = 0;
        int valid = 0;
        for (int i = 0; i < iterations; i++)
        {
            Vector3 from = location;
            float randomBinomial = Random.Range(-1f, 1f);
            from.x += radius * Mathf.Cos(theta) + randomBinomial * randomRadius;
            from.y += Random.value * 2f * randomRadius;
            from.z += radius * Mathf.Sin(theta) + randomBinomial * randomRadius;

            if (!IsInSameRoom(from, location))
                continue;
            valid++;

            Vector3 to = location;
            ;
            to.x += Random.Range(-1f, 1f) * characterSize.x;
            to.y += Random.value * characterSize.y;
            to.z += Random.Range(-1f, 1f) * characterSize.z;

            Vector3 direction = to - location;
            float distance = direction.magnitude;
            direction.Normalize();
            Ray ray = new Ray(location, direction);
            if (Physics.Raycast(ray, distance))
                hits++;
            theta = Mathf.Deg2Rad * deltaAngle;
        }
        return (float)(hits / valid);
    }

    public static float GetHeightQuality (Vector3 location, Vector3[] surroundings)
    {
        float maxQuality = 1f;
        float minQuality = -1f;
        float minHeight = Mathf.Infinity;
        float maxHeight = Mathf.NegativeInfinity;
        float height = location.y;
        foreach (Vector3 s in surroundings)
        {
            if (s.y > maxHeight)
                maxHeight = s.y;
            if (s.y < minHeight)
                minHeight = s.y;
        }
        float quality = (height-minHeight) / (maxHeight - minHeight);
        quality *= (maxQuality - minQuality);
        quality += minQuality;
        return quality;
    }

    public static void CondenseWaypoints(List<Waypoint> waypoints, float distanceWeight)
    {
        distanceWeight *= distanceWeight;
        waypoints.Sort();
        waypoints.Reverse();
        List<Waypoint> neighbours;
        foreach (Waypoint current in waypoints)
        {
            neighbours = new List<Waypoint>(current.neighbours);
            neighbours.Sort();
            foreach (Waypoint target in neighbours)
            {
                if (target.value > current.value)
                    break;
                if (!CanMove(current, target))
                    continue;

                Vector3 deltaPos = current.transform.position;
                deltaPos -= target.transform.position;
                deltaPos = Vector3.Cross(deltaPos, deltaPos);
                deltaPos *= distanceWeight;
                float deltaVal = current.value - target.value;
                deltaVal *= deltaVal;
                if (deltaVal < distanceWeight)
                {
                    neighbours.Remove(target);
                    waypoints.Remove(target);
                }
            }
        }
    }
}
