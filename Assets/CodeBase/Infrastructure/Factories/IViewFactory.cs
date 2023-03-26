using System.Collections.Generic;
using CodeBase.Domain;
using CodeBase.Presentation;

namespace CodeBase.Infrastructure
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
    }
}