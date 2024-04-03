using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IDeathView
    {
        Canvas Canvas { get; }
        ActionButton ResurrectButton { get; }
        ActionButton BackToMenuButton { get; }
        ActionButton DoubleRewardAdsButton { get; }
        void Initialize();
    }
}