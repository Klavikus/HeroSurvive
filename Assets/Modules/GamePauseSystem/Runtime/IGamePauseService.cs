using System;

namespace Modules.GamePauseSystem.Runtime
{
    public interface IGamePauseService
    {
        event Action Paused;
        event Action Resumed;
        void InvokeByAds(bool isCall);
        void InvokeByUI(bool isCall);
        void InvokeByFocusChanging(bool isCall);
    }
}