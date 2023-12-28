
public struct BombEvent : IGameEvent
{
    public readonly int Rank;

    public BombEvent(int rank) => Rank = rank;
}