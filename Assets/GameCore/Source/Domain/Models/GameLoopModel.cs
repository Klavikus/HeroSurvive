using System;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Domain.Models
{
    public class GameLoopModel
    {
        public event Action<HeroData> StartLevelInvoked;
        public event Action CloseLevelInvoked;
        public event Action PlayerResurrected;

        public void InvokeStartLevel(HeroData heroData) =>
            StartLevelInvoked?.Invoke(heroData);
        public void InvokeLevelClose() =>
            CloseLevelInvoked?.Invoke();
        public void ResurrectPlayer() =>
            PlayerResurrected?.Invoke();
    }
}