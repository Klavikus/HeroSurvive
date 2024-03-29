using System.Collections.Generic;
using CodeBase.GameCore.Domain.Models;
using CodeBase.GameCore.Infrastructure.Services;
using CodeBase.GameCore.Presentation.Views;
using CodeBase.GameCore.Presentation.Views.HeroSelector;
using CodeBase.GameCore.Presentation.Views.Upgrades;

namespace CodeBase.GameCore.Infrastructure.Factories
{
    public interface IViewFactory : IService
    {
        HeroSelectorView CreateHeroSelectorView();
        HeroView[] CreateHeroViews();
        PropertyView[] CreatePropertyViews();
        StartMenuView CreateStartMenu();
        UpgradesSelectorView CreateUpgradesSelectorView();
        UpgradeView[] CreateUpgradeViews();
        UpgradeLevelView[] CreateUpgradeLevelViews(int count);
        CurrencyView CreateCurrencyView();
        LeaderBoardScoreView[] CreateLeaderBoardScoreViews(IReadOnlyList<LeaderBoard> leaderBoards);
        LeaderBoardScoreView CreateLeaderBoardScoreView();
        SettingsView CreateSettingsView();
    }
}