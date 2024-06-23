using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.Common.Utils
{
    public class MultiCallHandler : IMultiCallHandler
    {
        private readonly Dictionary<string, bool> _callers = new();

        public event Action Called;
        public event Action Released;

        public bool IsCalled { get; private set; }

        public void Call(string key)
        {
            _callers[key] = true;

            CheckStatus();
        }

        public void Release(string key)
        {
            _callers[key] = false;

            CheckStatus();
        }

        public void Reset()
        {
            _callers.Clear();
            IsCalled = false;
            Released?.Invoke();
        }

        public bool CheckCallStatus(string key) =>
            _callers[key];

        private void CheckStatus()
        {
            bool isAnyActiveCall = _callers.Values.Any(caller => caller);

            if (isAnyActiveCall == IsCalled)
                return;

            IsCalled = isAnyActiveCall;

            if (IsCalled)
                Called?.Invoke();
            else
                Released?.Invoke();
        }
    }
}