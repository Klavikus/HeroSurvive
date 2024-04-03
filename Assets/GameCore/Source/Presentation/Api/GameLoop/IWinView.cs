using Modules.UIComponents.Runtime.Implementations;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IWinView
    {
        Canvas Canvas { get; }
        ActionButton ContinueButton { get; }
        ActionButton DoubleRewardButton { get; }
        ActionCounter KillCounter { get; }
        ActionCounter GoldCounter { get; }
        void Initialize();
    }
}