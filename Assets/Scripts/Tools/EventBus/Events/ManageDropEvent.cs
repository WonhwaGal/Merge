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
