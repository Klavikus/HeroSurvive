using GameCore.Source.Presentation.Api;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Core
{
    public class PauseView : ViewBase, IPauseView
    {
        [field: SerializeField] public Canvas MainCanvas { get; private set; }
        [field: SerializeField] public ActionButton CloseButton { get; private set; }
        [field: SerializeField] public ActionButton ResumeButton { get; private set; }
        [field: SerializeField] public ActionButton SettingsButton { get; private set; }
        [field: SerializeField] public ActionButton ExitLevelButton { get; private set; }

        public void Initialize()
        {
            CloseButton.Initialize();
            ResumeButton.Initialize();
            SettingsButton.Initialize();
            ExitLevelButton.Initialize();
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