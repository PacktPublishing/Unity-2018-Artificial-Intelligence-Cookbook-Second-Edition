using UnityEngine;
using System.Collections;

public class QLearning : MonoBehaviour
{

    public QValueStore store;


    private GameAction GetRandomAction(GameAction[] actions)
    {
        int n = actions.Length;
        return actions[Random.Range(0, n)];
    }

    public IEnumerator Learn(
            ReinforcementProblem problem,
            int numIterations,
            float alpha,
            float gamma,
            float rho,
            float nu)
    {
        if (store == null)
            yield break;

        GameState state = problem.GetRandomState();
        for (int i = 0; i < numIterations; i++)
        {
            yield return null;
            if (Random.value < nu)
                state = problem.GetRandomState();
            GameAction[] actions;
            actions = problem.GetAvailableActions(state);
            GameAction action;
            if (Random.value < rho)
                action = GetRandomAction(actions);
            else
                action = store.GetBestAction(state);
            float reward = 0f;
            GameState newState;
            newState = problem.TakeAction(state, action, ref reward);
            float q = store.GetQValue(state, action);
            GameAction bestAction = store.GetBestAction(newState);
            float maxQ = store.GetQValue(newState, bestAction);
            // perform QLearning
            q = (1f - alpha) * q + alpha * (reward + gamma * maxQ);
            store.StoreQValue(state, action, q);
            state = newState;
        }
        yield break;
    }


}
