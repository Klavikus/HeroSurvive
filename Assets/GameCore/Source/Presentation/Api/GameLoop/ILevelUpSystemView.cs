using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface ILevelUpSystemView
    {
        Canvas Canvas { get; }
        Image LevelCompletionImage { get; }
        ActionButton ContinueButton { get; }
        ActionButton ReRollButton { get; }
        IAbilityUpgradeView[] AbilityUpgradeViews { get; }
        void Initialize();
    }
}