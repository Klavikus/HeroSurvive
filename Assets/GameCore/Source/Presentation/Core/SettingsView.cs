using GameCore.Source.Presentation.Api;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core
{
    public class SettingsView : ViewBase, ISettingsView
    {
        [field: SerializeField] public Canvas MainCanvas { get; private set; }
        [field: SerializeField] public Slider MasterAudio { get; private set; }
        [field: SerializeField] public Slider MusicAudio { get; private set; }
        [field: SerializeField] public Slider VfxAudio { get; private set; }
        [field: SerializeField] public ActionButton ExitButton { get; private set; }

        public void Initialize()
        {
            ExitButton.Initialize();
        }

        public void Show()
        {
            MainCanvas.enabled = true;
        }

        public void Hide()
        {
            MainCanvas.enabled = false;
        }
    }
}