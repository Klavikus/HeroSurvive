using System;
using CodeBase.Domain.Data;
using UnityEngine;

namespace CodeBase.Domain.EntityComponents
{
    public abstract class InputHandler : MonoBehaviour
    {
        public event Action<InputData> InputUpdated;

        protected void InvokeInputUpdated(InputData inputData) =>
            InputUpdated?.Invoke(inputData);
    }
}