using UnityEngine;
using System.Collections;

public enum AHRState
{
    ATTACK,
    DEFEND,
    IDLE
}

public class AirHockeyRival : MonoBehaviour
{
    public GameObject puck;
    public GameObject paddle;
    public string goalWallTag = "GoalWall";
    public string sideWallTag = "SideWall";
    [Range(1, 10)]
    public int maxHits;
    float puckWidth;
    Renderer puckMesh;
    Rigidbody puckBody;
    AgentBehavior agent;
    Seek seek;
    Leave leave;
    AHRState state;
    bool hasAttacked;


    public void Awake()
    {
        puckMesh = puck.GetComponent<Renderer>();
        puckBody = puck.GetComponent<Rigidbody>();
        agent = paddle.GetComponent<AgentBehavior>();
        seek = paddle.GetComponent<Seek>();
        leave = paddle.GetComponent<Leave>();
        if (seek.target == null)
            seek.target = new GameObject();
        if (leave.target == null)
            leave.target = new GameObject();
        puckWidth = puckMesh.bounds.extents.z;
        state = AHRState.IDLE;
        hasAttacked = false;
    }

    public void Update()
    {
        switch (state)
        {
            case AHRState.ATTACK:
                Attack();
                break;
            default:
            case AHRState.IDLE:
                agent.enabled = false;
                break;
            case AHRState.DEFEND:
                Defend();
                break;
        }
        AttackReset();
    }

    public void SetState(AHRState newState)
    {
        state = newState;
    }

    private void Attack()
    {
        if (hasAttacked)
            return;
        agent.enabled = true;
        float dist = DistanceToPuck();
        if (dist > leave.dangerRadius)
        {
            Vector3 newPos = puck.transform.position;
            newPos.z = paddle.transform.position.z;
            seek.target.transform.position = newPos;
            seek.enabled = true;
            return;
            
        }
        hasAttacked = true;
        seek.enabled = false;
        Vector3 paddlePos = paddle.transform.position;
        Vector3 puckPos = puck.transform.position;
        Vector3 runPos = paddlePos - puckPos;
        runPos = runPos.normalized * 0.1f;
        runPos += paddle.transform.position;
        leave.target.transform.position = runPos;
        leave.enabled = true;
    }

    private float DistanceToPuck()
    {
        Vector3 puckPos = puck.transform.position;
        Vector3 paddlePos = paddle.transform.position;
        return Vector3.Distance(puckPos, paddlePos);
    }

    private void AttackReset()
    {
        float dist = DistanceToPuck();
        if (hasAttacked && dist < leave.dangerRadius)
            return;
        hasAttacked = false;
        leave.enabled = false;
    }
    
    private void Defend()
    {
        agent.enabled = true;
        seek.enabled = true;
        leave.enabled = false;
        Vector3 puckPos = puckBody.position;
        Vector3 puckVel = puckBody.velocity;
        Vector3 targetPos = Predict(puckPos, puckVel, 0);
        seek.target.transform.position = targetPos;
    }

    private Vector3 Predict(Vector3 position, Vector3 velocity, int numHit)
    {
        if (numHit == maxHits)
            return position;
        RaycastHit[] hits = Physics.RaycastAll(position, velocity.normalized);
        RaycastHit hit;
        foreach (RaycastHit h in hits)
        {
            string tag = h.collider.tag;
            if (tag.Equals(goalWallTag))
            {
                position = h.point;
                position += (h.normal * puckWidth);
                return position;
            }
            if (tag.Equals(sideWallTag))
            {
                hit = h;
                position = hit.point + (hit.normal * puckWidth);
                Vector3 u = hit.normal;
                u *= Vector3.Dot(velocity, hit.normal);
                Vector3 w = velocity - u;
                velocity = w - u;
                break;
            }
        }
        return Predict(position, velocity, numHit + 1);
    }
}
