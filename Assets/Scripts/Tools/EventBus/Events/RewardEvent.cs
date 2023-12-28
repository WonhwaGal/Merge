
public struct RewardEvent : IGameEvent
{
    public readonly int Rank;

    public RewardEvent(int rank) => Rank = rank;
}