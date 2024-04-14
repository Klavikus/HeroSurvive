using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Api.MainMenu
{
    public interface ILeaderBoardsView
    {
        Transform ScoreViewsContainer { get; }
        ActionButton CloseButton { get; }
        ILeaderBoardScoreView PlayerLeaderBoardScoreView { get; }
        void Show();
        void Hide();
    }
}