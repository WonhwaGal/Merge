using System;
using System.Collections.Generic;
using UnityEngine;

public class ContainerBank
{
    private Dictionary<Type, IDisposable> _bankData = new();

    public void Add<T>(Action<T> method) where T : struct, IGameEvent
    {
        if (!_bankData.ContainsKey(typeof(T)))
            _bankData.Add(typeof(T), new EventContainer<T>());

        ((IEventContainer<T>)_bankData[typeof(T)]).OnEvent += method;
    }

    public void Remove<T>(Action<T> method) where T : struct, IGameEvent
    {
        if (_bankData.ContainsKey(typeof(T)))
            ((IEventContainer<T>)_bankData[typeof(T)]).OnEvent -= method;
    }

    public bool TryGetEventContainer<T>(out IEventContainer<T> eventContainer) where T : struct, IGameEvent
    {
        eventContainer = null;
        if (_bankData.ContainsKey(typeof(T)))
        {
            eventContainer = (IEventContainer<T>)_bankData[typeof(T)];
            return true;
        }
        else
        {
            Debug.Log("Event was not found");
        }

        return false;
    }

    public void Dispose()
    {
        foreach (var value in _bankData.Values)
            value.Dispose();

        _bankData.Clear();
        GC.SuppressFinalize(this);
    }
}