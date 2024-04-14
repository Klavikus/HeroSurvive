using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Api.GameLoop
{
    public interface ILevelUpSystemView
    {
        Image LevelCompletionImage { get; }
        ActionButton ContinueButton { get; }
        ActionButton ReRollButton { get; }
        IAbilityUpgradeView[] AbilityUpgradeViews { get; }
        void Initialize();
        void Show();
        void Hide();
    }
}