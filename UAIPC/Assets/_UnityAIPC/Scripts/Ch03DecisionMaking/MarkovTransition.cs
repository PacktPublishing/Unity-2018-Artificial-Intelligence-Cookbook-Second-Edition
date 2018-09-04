using UnityEngine;
using System.Collections;

public class MarkovTransition : MonoBehaviour
{
    public Matrix4x4 matrix;
    public MonoBehaviour action;

    public virtual bool IsTriggered()
    {
        // implementation details here
        return false;
    }
}
