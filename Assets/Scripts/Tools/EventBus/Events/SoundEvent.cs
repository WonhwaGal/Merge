
public struct SoundEvent : IGameEvent
{
    public SoundType SoundType;
    public bool TurnOn;

    public SoundEvent(SoundType soundType, bool turnOn)
    {
        SoundType = soundType;
        TurnOn = turnOn;
    }
}