using System;

public class EventContainer<T> : IEventContainer<T> where T : struct, IGameEvent
{
    private bool _disposed;

    public event Action<T> OnEvent;

    public void OnSend(T eventT) => OnEvent?.Invoke(eventT);

    public void Dispose()
    {
        if (_disposed) // _disposed never set to false
            return;

        OnEvent = null;
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
