using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFSMWandering : StateMachineBehaviour
{
	public float pursueDistance = 3f;
	private Wander _wander;
	private Seek _pursue;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(
			Animator animator,
			AnimatorStateInfo stateInfo,
			int layerIndex)
	{
		AgentBehavior[] behaviors;
		GameObject gameObject = animator.gameObject;
		behaviors = gameObject.GetComponents<AgentBehavior>();
		foreach (AgentBehavior b in behaviors)
			b.enabled = false;
		
		_wander = gameObject.GetComponent<Wander>();
		_pursue = gameObject.GetComponent<Seek>();
		if (_wander == null || _pursue == null)
			return;
		_wander.enabled = true;
		animator.gameObject.name = "Wandering";
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(
			Animator animator,
			AnimatorStateInfo stateInfo,
			int layerIndex)
	{
		Vector3 targetPos, agentPos;
		targetPos = _pursue.target.transform.position;
		agentPos = animator.transform.position;
		float distance = Vector3.Distance(targetPos, agentPos);
		if (distance > pursueDistance)
			return;
		animator.SetTrigger("Pursue");
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
