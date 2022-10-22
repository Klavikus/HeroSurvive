using System;
using UnityEngine;

public class InputController : InputHandler
{
    private readonly string Horizontal = "Horizontal";
    private readonly string Vertical = "Vertical";

    private void Update()
    {
        float horizontal = SimpleInput.GetAxis(Horizontal);
        float vertical = SimpleInput.GetAxis(Vertical);

        InvokeInputUpdated(new InputData(horizontal, vertical));
    }
}

public abstract class InputHandler : MonoBehaviour
{
    public event Action<InputData> InputUpdated;

    protected void InvokeInputUpdated(InputData inputData) =>
        InputUpdated?.Invoke(inputData);
}

public struct InputData
{
    public float Horizontal;
    public float Vertical;

    public InputData(float horizontal, float vertical)
    {
        Horizontal = horizontal;
        Vertical = vertical;
    }
}