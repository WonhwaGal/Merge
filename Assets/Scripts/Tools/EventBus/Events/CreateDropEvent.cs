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
