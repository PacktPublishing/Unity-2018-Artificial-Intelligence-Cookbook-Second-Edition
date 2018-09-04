using UnityEngine;
using System.Collections;

public class ProjectileShooter : MonoBehaviour {

    public GameObject projectileSimple;
    public float speed;
    public Vector3 direction;

    void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 position = transform.position + Vector3.up * 2.0f;
            GameObject projectile;
            projectile = Instantiate(projectileSimple, position, new Quaternion()) as GameObject;
            Projectile p = projectile.GetComponent<Projectile>();
            p.Set(position, direction, speed);
        }
    }
}
