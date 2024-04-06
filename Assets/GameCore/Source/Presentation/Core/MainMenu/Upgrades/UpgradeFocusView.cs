using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core.MainMenu.Upgrades
{
    public class UpgradeFocusView : ViewBase, IUpgradeFocusView
    {
        [field: SerializeField] public TMP_Text Name { get; private set; }
        [field: SerializeField] public TMP_Text Description { get; private set; }
        [field: SerializeField] public TMP_Text UpgradePrice { get; private set; }
        [field: SerializeField] public TMP_Text SellPrice { get; private set; }
        [field: SerializeField] public ActionButton UpgradeButton { get; private set; }
        [field: SerializeField] public ActionButton SellButton { get; private set; }
        [field: SerializeField] public Image Icon { get; private set; }
        [field: SerializeField] public RectTransform UpgradeLevelViewsContainer { get; private set; }

        public void Initialize()
        {
            UpgradeButton.Initialize();
            SellButton.Initialize();
        }
    }
}