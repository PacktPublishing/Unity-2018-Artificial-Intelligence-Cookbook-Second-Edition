using UnityEngine;
using System.Collections;

public class TFRBar : MonoBehaviour
{
    [HideInInspector]
    public int barId;
    public float barSpeed;
    public float attackDegrees = 30f;
    public float defendDegrees = 0f;
    public float openDegrees = 90f;

    public GameObject ball;
    private Coroutine crTransition;
    private bool isLocked;

    void Awake()
    {
        crTransition = null;
        isLocked = false;
    }

    public void SetState(TFRState state, float speed = 0f)
    {
        if (isLocked)
            return;
        // optional
        isLocked = true;
        if (speed == 0)
            speed = barSpeed;
        float degrees = 0f;
        switch(state)
        {
            case TFRState.ATTACK:
                degrees = attackDegrees;
                break;
            default:
            case TFRState.DEFEND:
                degrees = defendDegrees;
                break;
            case TFRState.OPEN:
                degrees = openDegrees;
                break;
        }
        if (crTransition != null)
            StopCoroutine(crTransition);
        crTransition = StartCoroutine(Rotate(degrees, speed));
    }

    public IEnumerator Rotate(float target, float speed)
    {
        while (transform.rotation.x != target)
        {
            Quaternion rot = transform.rotation;
            if (Mathf.Approximately(rot.x, target))
            {
                rot.x = target;
                transform.rotation = rot;
            }
            float vel = target - rot.x;
            rot.x += speed * Time.deltaTime * vel;
            yield return null;
        }
        isLocked = false;
        transform.rotation = Quaternion.identity;
    }

    public void Slide(float target, float speed)
    {
        Vector3 targetPos = transform.position;
        targetPos.x = target;
        Vector3 trans = transform.position - targetPos;
        trans *= speed * Time.deltaTime;
        transform.Translate(trans, Space.World);
    }

}
