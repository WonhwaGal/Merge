using Code.DropLogic;

public struct ManageDropEvent : IGameEvent
{
    public readonly DropBase Drop;
    public readonly bool ReturnToPool;
    public readonly bool WithEffects;

    public ManageDropEvent(DropBase drop, bool returnToPool, bool withEffects)
    {
        Drop = drop;
        ReturnToPool = returnToPool;
        WithEffects = withEffects;
    }
}
