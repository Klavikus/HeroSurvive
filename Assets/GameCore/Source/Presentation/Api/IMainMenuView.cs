using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Api
{
    public interface IMainMenuView
    {
        Canvas Canvas { get; }
        ActionButton StartButton { get; }
    }
}