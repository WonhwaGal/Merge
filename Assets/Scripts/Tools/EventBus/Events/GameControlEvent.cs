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
