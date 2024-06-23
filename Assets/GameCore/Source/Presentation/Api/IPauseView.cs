using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Api
{
    public interface IPauseView
    {
        Canvas MainCanvas { get; }
        ActionButton CloseButton { get; }
        ActionButton ResumeButton { get; }
        ActionButton SettingsButton { get; }
        ActionButton ExitLevelButton { get; }
        void Show();
        void Hide();
        void Initialize();
    }
}