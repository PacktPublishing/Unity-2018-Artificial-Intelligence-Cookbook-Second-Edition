using UnityEngine;
using System.Collections;

public class ProjectileDirector : MonoBehaviour {

    public GameObject projectileSimple;
    public float speed = 7.0f;
    public float shootDelay = 0.5f;

    public GameObject target;
    public float targetSpeed = 3.0f;

    Vector3 targetDirection;
    float shootTimer = 0.0f;
    Vector3 direction;
    Renderer myRenderer;

    void Start () {
        targetDirection = Vector3.right;
        myRenderer = GameObject.Find("Capsule").GetComponent<MeshRenderer>();
    }

    void Update () {
        if (target.transform.position.x > 10.0f)
            targetDirection = Vector3.left;
        if (target.transform.position.x < -10.0f)
            targetDirection = Vector3.right;
        target.transform.Translate(targetDirection * targetSpeed * Time.deltaTime);

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootDelay)
        {
            shootTimer -= shootDelay;
            Shoot();
        }
    }

    void Shoot () {
        Vector3 firePos = transform.position;
        firePos.y = myRenderer.bounds.max.y;
        Vector3 startPos = transform.position;
        Vector3 endPos = target.transform.position;
        Vector3 direction = Projectile.GetFireDirection(startPos, endPos, speed);
        GameObject projectile = Instantiate(projectileSimple, firePos, new Quaternion()) as GameObject;
        projectile.GetComponent<Projectile>().Set(firePos, direction, speed);
    }
}
