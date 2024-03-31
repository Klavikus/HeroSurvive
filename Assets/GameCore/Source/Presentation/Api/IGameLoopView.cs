using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Api
{
    public interface IGameLoopView
    {
        Canvas Canvas { get; }
        ActionButton CloseButton { get; }
    }
}