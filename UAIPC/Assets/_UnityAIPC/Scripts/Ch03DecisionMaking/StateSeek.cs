using UnityEngine;

public class StateSeek : State
{
    public float speed;
    public GameObject targetToFollow;

    public override void Awake()
    {
        base.Awake();
        ConditionTargetFar ctf = new ConditionTargetFar();
        ctf.origin = this.gameObject;
        ctf.target = targetToFollow;
        ctf.maxDistance = 7f;
        State st = GetComponent<StateIdle>();
        Transition transition = new Transition();
        transition.condition = ctf;
        transition.target = st;
        transitions.Add(transition);
    }

    public override void Update()
    {
        if (targetToFollow == null)
            return;
        Vector3 direction = targetToFollow.transform.position - transform.position;
        direction.Normalize();
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
