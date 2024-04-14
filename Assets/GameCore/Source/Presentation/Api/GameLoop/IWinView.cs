using Modules.UIComponents.Runtime.Implementations;
using Modules.UIComponents.Runtime.Implementations.Buttons;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IWinView
    {
        ActionButton ContinueButton { get; }
        ActionButton DoubleRewardButton { get; }
        ActionCounter KillCounter { get; }
        ActionCounter GoldCounter { get; }
        void Initialize();
        void Show();
        void Hide();
    }
}