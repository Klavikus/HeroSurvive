using System;
using UnityEngine;

namespace CodeBase.Domain
{
    public abstract class InputHandler : MonoBehaviour
    {
        public event Action<InputData> InputUpdated;

        protected void InvokeInputUpdated(InputData inputData) =>
            InputUpdated?.Invoke(inputData);
    }
}