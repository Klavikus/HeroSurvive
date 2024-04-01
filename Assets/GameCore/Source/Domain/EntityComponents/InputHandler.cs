using System;
using GameCore.Source.Domain.Data;
using UnityEngine;

namespace GameCore.Source.Domain.EntityComponents
{
    public abstract class InputHandler : MonoBehaviour
    {
        private bool _isEnabled;
        public event Action<InputData> InputUpdated;

        protected void InvokeInputUpdated(InputData inputData)
        {
            if (_isEnabled == false)
                return;

            InputUpdated?.Invoke(inputData);
        }

        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
            InputUpdated?.Invoke(new InputData(0, 0));
        }
    }
}