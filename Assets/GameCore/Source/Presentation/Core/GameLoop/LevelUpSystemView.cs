using System.Linq;
using CodeBase.GameCore.Presentation.Views;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core.GameLoop
{
    public class LevelUpSystemView : ViewBase, ILevelUpSystemView
    {
        [SerializeField] private AbilityUpgradeView[] _abilityUpgradeViews;
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public Image LevelCompletionImage { get; private set; }
        [field: SerializeField] public ActionButton ContinueButton { get; private set; }
        [field: SerializeField] public ActionButton ReRollButton { get; private set; }
        public IAbilityUpgradeView[] AbilityUpgradeViews => _abilityUpgradeViews.Cast<IAbilityUpgradeView>().ToArray();

        public void Initialize()
        {
            ContinueButton.Initialize();
            ReRollButton.Initialize();
        }
    }
}