using Modules.UIComponents.Runtime.Implementations.Buttons;

namespace GameCore.Source.Presentation.Api.MainMenu
{
    public interface IMainMenuView
    {
        ActionButton StartButton { get; }
        ActionButton LeaderBoardButton { get; }
        ActionButton PersistentUpgradesButton { get; }
        void Show();
        void Hide();
    }
}