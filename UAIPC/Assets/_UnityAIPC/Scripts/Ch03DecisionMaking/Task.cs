using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Task : MonoBehaviour
{
    public List<Task> children;
    protected bool result = false;
    protected bool isFinished = false;

    public virtual void SetResult(bool r)
    {
        result = r;
        isFinished = true;
    }

    public IEnumerator PrintNumber(int n)
    {
        Debug.Log(n);
        yield break;
    }

    public virtual IEnumerator Run()
    {
        SetResult(true);
        yield break;
    }

    public virtual IEnumerator RunTask()
    {
        yield return StartCoroutine(Run());
    }
}
