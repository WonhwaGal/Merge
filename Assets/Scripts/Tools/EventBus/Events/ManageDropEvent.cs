using Code.DropLogic;

public struct ManageDropEvent : IGameEvent
{
    public readonly DropObject Drop;
    public readonly bool ReturnToPool;

    public ManageDropEvent(DropObject drop, bool returnToPool)
    {
        Drop = drop;
        ReturnToPool = returnToPool;
    }
}

public struct GameControlEvent: IGameEvent
{
    public readonly GameAction ActionToDo;
    public readonly bool RestartWithRetry;
    
    public GameControlEvent(GameAction actionToDo, bool restartWithRetry)
    {
        ActionToDo = actionToDo;
        RestartWithRetry = restartWithRetry;
    }
}

public struct CreateDropEvent : IGameEvent
{
    public readonly bool QueueMoved;
    public readonly int CurrentRank;

    public CreateDropEvent(bool queueMoved, int rank)
    {
        QueueMoved = queueMoved;
        CurrentRank = rank;
    }
}

public struct SaveEvent : IGameEvent
{
    public readonly int CurrentScore;
    public readonly bool OnlyScore;

    public SaveEvent(int currentScore, bool onlyScore)
    {
        CurrentScore = currentScore;
        OnlyScore = onlyScore;
    }
}