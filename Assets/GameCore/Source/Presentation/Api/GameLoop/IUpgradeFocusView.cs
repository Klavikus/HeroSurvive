using Modules.UIComponents.Runtime.Implementations.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface IUpgradeFocusView
    {
        TMP_Text Name { get; }
        TMP_Text Description { get; }
        TMP_Text UpgradePrice { get; }
        TMP_Text SellPrice { get; }
        ActionButton UpgradeButton { get; }
        ActionButton SellButton { get; }
        Image Icon { get; }
        RectTransform UpgradeLevelViewsContainer { get; }
        void Initialize();
    }
}