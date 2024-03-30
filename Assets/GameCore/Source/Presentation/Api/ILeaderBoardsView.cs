using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Api
{
    public interface ILeaderBoardsView
    {
        Canvas MainCanvas { get; }
        Transform ScoreViewsContainer { get; }
        ActionButton CloseButton { get; }
        ILeaderBoardScoreView PlayerLeaderBoardScoreView { get; }
    }
}