using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace CodeBase.GameCore.Presentation.Views.Upgrades
{
    public class UpgradesSelectorView : ViewBase, IUpgradesSelectorView
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
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
    }
}