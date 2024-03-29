using System;
using CodeBase.GameCore.Domain.Data;
using UnityEngine;

namespace CodeBase.GameCore.Domain.EntityComponents
{
    public abstract class InputHandler : MonoBehaviour
    {
        public event Action<InputData> InputUpdated;

        protected void InvokeInputUpdated(InputData inputData) =>
            InputUpdated?.Invoke(inputData);
    }
}