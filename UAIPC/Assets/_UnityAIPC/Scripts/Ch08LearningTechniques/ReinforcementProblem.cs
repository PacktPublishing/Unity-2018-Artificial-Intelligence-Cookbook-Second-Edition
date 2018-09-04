public struct GameState
{
    // TODO
    // your state definition here
}

public struct GameAction
{
    // TODO
    // your action definition here
}


public class ReinforcementProblem
{
    public virtual GameState GetRandomState()
    {
        // TODO
        // Define your own behaviour
        return new GameState();
    }

    public virtual GameAction[] GetAvailableActions(GameState s)
    {
        // TODO
        // Define your own behaviour
        return new GameAction[0];
    }

    public virtual GameState TakeAction(
            GameState s,
            GameAction a,
            ref float reward)
    {
        // TODO
        // Define your own behaviour
        reward = 0f;
        return new GameState();
    }
}


