using System;
using CodeBase.Domain.Data;

namespace CodeBase.MVVM.Models
{
    public class GameLoopModel
    {
        public event Action<HeroData> StartLevelInvoked;
        public event Action CloseLevelInvoked;
        public event Action PlayerResurrected;
        public event Action UserNameShowInvoked;

        public void InvokeStartLevel(HeroData heroData) => StartLevelInvoked?.Invoke(heroData);
        public void InvokeLevelClose() => CloseLevelInvoked?.Invoke();
        public void InvokeUserNameShow() => UserNameShowInvoked?.Invoke();
        public void ResurrectPlayer() => PlayerResurrected?.Invoke();
    }
}