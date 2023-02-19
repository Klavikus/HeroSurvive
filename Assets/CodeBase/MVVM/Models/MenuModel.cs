using System;

namespace CodeBase.MVVM.Models
{
    public class MenuModel
    {
        public event Action OpenedHeroSelection;
        public event Action OpenedUpgradeSelection;
        public event Action ClosedHeroSelection;
        public event Action ClosedUpgradeSelection;
        public event Action LeaderBoardShowInvoked;
        public event Action LeaderBoardHideInvoked;


        public void EnableHeroSelector() => OpenedHeroSelection?.Invoke();
        public void DisableHeroSelector() => ClosedHeroSelection?.Invoke();
        public void EnableUpgradeSelection() => OpenedUpgradeSelection?.Invoke();
        public void DisableUpgradeSelection() => ClosedUpgradeSelection?.Invoke();

        public void InvokeLeaderBoardShow() => LeaderBoardShowInvoked?.Invoke();
        public void InvokeLeaderBoardHide() => LeaderBoardHideInvoked?.Invoke();
    }
}