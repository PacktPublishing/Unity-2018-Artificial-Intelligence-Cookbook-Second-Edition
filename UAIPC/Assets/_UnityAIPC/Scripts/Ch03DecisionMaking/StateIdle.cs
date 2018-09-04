using UnityEngine;

public class StateIdle : State
{
    public override void Awake()
    {
        base.Awake();
        ConditionTargetClose ctc = new ConditionTargetClose();
        ctc.origin = this.gameObject;
        ctc.target = GameObject.FindGameObjectWithTag("Player");
        ctc.minDistance = 3f;
        State st = GetComponent<StateSeek>();
        Transition transition = new Transition();
        transition.condition = ctc;
        transition.target = st;
        transitions.Add(transition);
    }
}
