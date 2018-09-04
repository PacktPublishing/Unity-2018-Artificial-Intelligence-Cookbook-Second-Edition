using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private bool set = false;
    private Vector3 firePos;
    private Vector3 direction;
    private float speed;
    private float timeElapsed;

    /// <summary>
    /// If false, gameobject is alive but this component will be disabled.
    /// </summary>
    private bool destroyObjectOnFinish = true;

    public Vector3 FirePosition
    {
        get { return firePos; }
    }

    public Vector3 Direction
    {
        get { return direction; }
    }

    public float Speed
    {
        get { return speed; }
    }

    void Update ()
    {
        if (!set)
            return;
        timeElapsed += Time.deltaTime;
        transform.position = firePos + direction * speed * timeElapsed;
        transform.position += Physics.gravity * (timeElapsed * timeElapsed) / 2.0f;
        // extra validation for cleaning the scene
        //if (transform.position.y < -1.0f)
        //    Destroy(this.gameObject);// or set = false; and hide it
        if (transform.position.y < 0f)
        {
            if (destroyObjectOnFinish)
                Destroy(this.gameObject);// or set = false; and hide it
            else
                this.enabled = false;
        }
    }

    public void Set (Vector3 firePos, Vector3 direction, float speed, bool destroyObjectWhenFinished = true)
    {
        this.firePos = firePos;
        this.direction = direction.normalized;
        this.speed = speed;
        transform.position = firePos;
        set = true;
        this.destroyObjectOnFinish = destroyObjectWhenFinished;
        this.timeElapsed = 0f;
    }

    public float GetLandingTime (float height = 0.0f)
    {
        Vector3 position = transform.position;
        float time = 0.0f;
        float valueInt = (direction.y * direction.y) * (speed * speed);
        valueInt = valueInt - (Physics.gravity.y * 2 * (position.y - height));
        valueInt = Mathf.Sqrt(valueInt);
        float valueAdd = (-direction.y) * speed;
        float valueSub = (-direction.y) * speed;
        valueAdd = (valueAdd + valueInt) / Physics.gravity.y;
        valueSub = (valueSub - valueInt) / Physics.gravity.y;
        if (float.IsNaN(valueAdd) && !float.IsNaN(valueSub))
            return valueSub;
        else if (!float.IsNaN(valueAdd) && float.IsNaN(valueSub))
            return valueAdd;
        else if (float.IsNaN(valueAdd) && float.IsNaN(valueSub))
            return -1.0f;
        time = Mathf.Max(valueAdd, valueSub);
        return time;
    }
    
    public Vector3 GetLandingPos (float height = 0.0f)
    {
        Vector3 landingPos = Vector3.zero;
        float time = GetLandingTime();
        if (time < 0.0f)
            return landingPos;
        landingPos.y = height;
        landingPos.x = firePos.x + direction.x * speed * time;
        landingPos.z = firePos.z + direction.z * speed * time;
        return landingPos;
    }

    public static Vector3 GetFireDirection (Vector3 startPos, Vector3 endPos, float speed)
    {
        Vector3 direction = Vector3.zero;
        Vector3 delta = endPos - startPos;
        float a = Vector3.Dot(Physics.gravity, Physics.gravity);
        float b = -4 * (Vector3.Dot(Physics.gravity, delta) + speed * speed);
        float c = 4 * Vector3.Dot(delta, delta);
        if (4 * a * c > b * b)
            return direction;
        float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2*a));
        float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2*a));
        float time;
        if (time0 < 0.0f)
        {
            if (time1 < 0)
                return direction;
            time = time1;
        }
        else
        {
            if (time1 < 0)
                time = time0;
            else
                time = Mathf.Min(time0, time1);
        }
        direction = 2 * delta - Physics.gravity * (time * time);
        direction = direction / (2 * speed * time);
        return direction;
    }
}
