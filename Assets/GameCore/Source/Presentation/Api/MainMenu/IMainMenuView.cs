using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Api.MainMenu
{
    public interface IMainMenuView
    {
        Canvas Canvas { get; }
        ActionButton StartButton { get; }
        ActionButton LeaderBoardButton { get; set; }
    }
}