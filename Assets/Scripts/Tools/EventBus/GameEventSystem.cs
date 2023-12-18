using System;


public static class GameEventSystem
{
    private static ContainerBank _bank = new ();

    public static void Subscribe<T>(Action<T> method) where T : struct, IGameEvent
    {
        _bank.Add<T>(method);
    }

    public static void UnSubscribe<T>(Action<T> method) where T : struct, IGameEvent
    {
        _bank.Remove<T>(method);
    }

    public static void Send<T>(T someEvent) where T : struct, IGameEvent
    {
        if (_bank.TryGetEventContainer<T>(out var eventContainer))
            eventContainer.OnSend(someEvent);
    }

    public static void Dispose()
    {
        _bank.Dispose();
    }
}