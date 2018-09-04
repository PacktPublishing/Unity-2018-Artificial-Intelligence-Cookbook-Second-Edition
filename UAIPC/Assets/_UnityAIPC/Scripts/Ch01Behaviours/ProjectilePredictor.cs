using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectilePredictor : MonoBehaviour {

    public GameObject projectileTemplate;
    public GameObject projectilePredictor;
    public float predictionHeight = 0.0f;
    public float speed;
    public Vector3 direction;
    List<GameObject> projectiles;
    Renderer myRenderer;
    Projectile p;
    void Awake () {
        projectiles = new List<GameObject>();
        myRenderer = GameObject.Find("Capsule").GetComponent<MeshRenderer>();
    }

    void Update () {
        if (projectiles.Count == 0)
        {
            Vector3 firePos = transform.position;
            firePos.y += this.myRenderer.bounds.size.y / 2.0f;
            direction = Random.insideUnitSphere;
            direction.y = Mathf.Abs(direction.y);
            speed = Random.Range(5.0f, 15.0f);
            GameObject p = Instantiate(projectileTemplate, firePos, new Quaternion()) as GameObject;
            p.GetComponent<Projectile>().Set(firePos, direction, speed);
            projectiles.Add(p);
        }
        else
        {
            p = projectiles[0].GetComponent<Projectile>();
            projectilePredictor.transform.position = p.GetLandingPos(predictionHeight);
            if (projectiles[0].transform.position.y < predictionHeight)
            {
                Destroy(projectiles[0]);
                projectiles.Clear();
                projectilePredictor.transform.position = Vector3.zero;
                p = null;
            }
        }
    }
}
