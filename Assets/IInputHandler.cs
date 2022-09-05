using System;

public interface IInputHandler
{
    public event Action<InputData> InputUpdated;
}