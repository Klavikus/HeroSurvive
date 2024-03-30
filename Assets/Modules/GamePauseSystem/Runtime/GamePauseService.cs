using System;
using Modules.Common.Utils;

namespace Modules.GamePauseSystem.Runtime
{
    public class GamePauseService : IGamePauseService
    {
        private readonly IMultiCallHandler _multiCallHandler;

        public GamePauseService(IMultiCallHandler multiCallHandler)
        {
            _multiCallHandler = multiCallHandler;

            _multiCallHandler.Called += () => Paused?.Invoke();
            _multiCallHandler.Released += () => Resumed?.Invoke();
        }

        public event Action Paused;
        public event Action Resumed;

        public void InvokeByAds(bool isCall) =>
            HandleInvoke(nameof(InvokeByAds), isCall);

        public void InvokeByUI(bool isCall) =>
            HandleInvoke(nameof(InvokeByUI), isCall);

        public void InvokeByFocusChanging(bool isCall) =>
            HandleInvoke(nameof(InvokeByFocusChanging), isCall);

        private void HandleInvoke(string invokeBy, bool isCall)
        {
            if (isCall)
                _multiCallHandler.Call(invokeBy);
            else
                _multiCallHandler.Release(invokeBy);
        }
    }
}