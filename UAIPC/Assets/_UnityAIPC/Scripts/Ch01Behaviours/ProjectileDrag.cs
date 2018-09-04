using UnityEngine;
using System.Collections;

public class ProjectileDrag : MonoBehaviour {
    private bool set = false;
    private Vector3 firePos;
    private Vector3 direction;
    private float speed;
    private float timeElapsed;
    private float drag;
    private Vector3 A;
    private Vector3 B;
    private Vector3 position;
    
    void Update ()
    {
        if (!set)
            return;
        timeElapsed += Time.deltaTime;
        transform.position = firePos + direction * speed * timeElapsed;
        transform.position += Physics.gravity * (timeElapsed * timeElapsed) / 2.0f;
        float exp_kt = Mathf.Exp(-(drag*timeElapsed));
        position = Physics.gravity * timeElapsed;
        position -= A * exp_kt;
        position /= drag;
        position += B;
        transform.position += position;
        // extra validation for cleaning the scene
        if (transform.position.y < -1.0f)
            Destroy(this.gameObject);// or set = false; and hide it
    }
    
    public void Set (Vector3 firePos, Vector3 direction, float speed, float drag)
    {
        this.firePos = firePos;
        this.direction = direction.normalized;
        this.speed = speed;
        this.drag = drag;
        timeElapsed = 0.0f;
        A = direction.normalized * speed;
        A -= Physics.gravity / drag;
        B = firePos - (A / drag);
        set = true;
    }
}
