using System.Collections.Generic;
using CodeBase.Domain.Models;
using CodeBase.Infrastructure.Services;
using CodeBase.Presentation.Views;
using CodeBase.Presentation.Views.HeroSelector;
using CodeBase.Presentation.Views.Upgrades;

namespace CodeBase.Infrastructure.Factories
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