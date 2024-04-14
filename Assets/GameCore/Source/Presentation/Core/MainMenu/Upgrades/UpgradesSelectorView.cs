using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Core.MainMenu.Upgrades
{
    public class UpgradesSelectorView : ViewBase, IUpgradesSelectorView
    {
        [field: SerializeField] public Canvas[] Canvases { get; private set; }
        [field: SerializeField] public Transform UpgradeViewsContainer { get; private set; }
        [field: SerializeField] public ActionButton CloseButton { get; private set; }
        [field: SerializeField] public ActionButton UserNameButton { get; private set; }
        [field: SerializeField] public int RowCount { get; private set; }
        [field: SerializeField] public int ColCount { get; private set; }

        public void Initialize()
        {
            CloseButton.Initialize();
            UserNameButton.Initialize();
        }

        public void Show()
        {
            foreach (Canvas canvas in Canvases)
                canvas.enabled = true;
        }

        public void Hide()
        {
            foreach (Canvas canvas in Canvases)
                canvas.enabled = false;
        }
    }
}