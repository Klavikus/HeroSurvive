using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Api
{
    public interface ISettingsView
    {
        Canvas MainCanvas { get; }
        Slider MasterAudio { get; }
        Slider MusicAudio { get; }
        Slider VfxAudio { get; }
        ActionButton ExitButton { get; }
        void Show();
        void Hide();
        void Initialize();
    }
}