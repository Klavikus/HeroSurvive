using CodeBase.MVVM.Views;
using CodeBase.MVVM.Views.HeroSelector;
using CodeBase.MVVM.Views.Upgrades;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    [CreateAssetMenu(menuName = "Create MainMenuConfiguration", fileName = "MainMenuConfiguration", order = 0)]
    public class MainMenuConfigurationSO : ScriptableObject
    {
        public HeroSelectorView HeroSelectorView;
        public HeroDescriptionView HeroDescriptionView;
        public BaseAbilityView BaseAbilityView;
        public StartMenuView StartMenuView;
        public UpgradesSelectorView UpgradesSelectorView;
        public UpgradeView UpgradeView;
        public UpgradeLevelView UpgradeLevelView;
        public CurrencyView CurrencyView;
        public LeaderBoardScoreView LeaderBoardScoreView;
    }
}