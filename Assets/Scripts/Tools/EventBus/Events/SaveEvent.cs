public struct SaveEvent : IGameEvent
{
    public readonly float CurrentScore;
    public readonly bool OnlyScore;

    public SaveEvent(float currentScore, bool onlyScore)
    {
        CurrentScore = currentScore;
        OnlyScore = onlyScore;
    }
}
