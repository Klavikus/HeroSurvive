using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.Views;
using CodeBase.MVVM.Views.HeroSelector;
using CodeBase.MVVM.Views.Upgrades;

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
        UserNameSetterView CreateUserNameView();
        LeaderBoardScoreView[] CreateLeaderBoardScoreViews(IReadOnlyList<LeaderBoard> leaderBoards);
        LeaderBoardScoreView CreateLeaderBoardScoreView();
    }
}