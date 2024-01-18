
public struct BackgroundEvent : IGameEvent
{
    public readonly int Index;
    public readonly bool ToApply;

    public BackgroundEvent(int index, bool toApply)
    {
        Index = index;
        ToApply = toApply;
    }
}