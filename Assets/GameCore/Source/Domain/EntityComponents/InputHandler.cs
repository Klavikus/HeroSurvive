using System;
using GameCore.Source.Domain.Data;
using UnityEngine;

namespace GameCore.Source.Domain.EntityComponents
{
    public abstract class InputHandler : MonoBehaviour
    {
        public event Action<InputData> InputUpdated;

        protected void InvokeInputUpdated(InputData inputData) =>
            InputUpdated?.Invoke(inputData);
    }
}