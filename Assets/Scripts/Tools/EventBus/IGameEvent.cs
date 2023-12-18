
public interface IGameEvent { }

public struct PauseEvent: IGameEvent
{
    public readonly bool IsPaused;

    public PauseEvent(bool value) => IsPaused = value;
}

