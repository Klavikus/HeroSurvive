using CodeBase.GameCore.Presentation.Views;
using CodeBase.GameCore.Presentation.Views.HeroSelector;
using CodeBase.GameCore.Presentation.Views.Upgrades;
using UnityEngine;

namespace CodeBase.GameCore.Configs
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
        public SettingsView SettingsView;
    }
}