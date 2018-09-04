using UnityEngine;
using System.Collections;

public class ActionChooser : MonoBehaviour
{
    public ActionGOB Choose(ActionGOB[] actions, GoalGOB[] goals)
    {
        ActionGOB bestAction;
        bestAction = actions[0];
        float bestValue = CalculateDiscontentment(actions[0], goals);
        float value;
        foreach (ActionGOB action in actions)
        {
            value = CalculateDiscontentment(action, goals);
            if (value < bestValue)
            {
                bestValue = value;
                bestAction = action;
            }
        }
        return bestAction;
    }

    public float CalculateDiscontentment(ActionGOB action, GoalGOB[] goals)
    {
        float discontentment = 0;
        foreach (GoalGOB goal in goals)
        {
            float newValue = goal.value + action.GetGoalChange(goal);
            newValue += action.GetDuration() * goal.GetChange();
            discontentment += goal.GetDiscontentment(newValue);
        }
        return discontentment;
    }
}
