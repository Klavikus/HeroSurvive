using Modules.UIComponents.Runtime.Implementations.Buttons;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IDeathView
    {
        ActionButton ResurrectButton { get; }
        ActionButton BackToMenuButton { get; }
        ActionButton DoubleRewardAdsButton { get; }
        void Initialize();
        void Show();
        void Hide();
    }
}