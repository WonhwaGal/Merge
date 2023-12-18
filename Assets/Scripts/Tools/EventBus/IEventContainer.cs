using System;

public interface IEventContainer<T> : IDisposable where T : IGameEvent
{
    public event Action<T> OnEvent;
    public void OnSend(T eventT);
}
