using System;
using System.Threading;
using Modules.UIComponents.Runtime.Interfaces;
using UnityEngine;

namespace Modules.UIComponents.Runtime.Implementations
{
    public abstract class ActionCounter : MonoBehaviour, IActionCounter
    {
        public event Action CountStarted;
        public event Action CountCompleted;

        public abstract void Initialize(float initialValue);
        public abstract void Count(float targetValue);

        protected void InvokeCountStarted() =>
            CountStarted?.Invoke();

        protected void InvokeCountCompleted() =>
            CountCompleted?.Invoke();
    }
}