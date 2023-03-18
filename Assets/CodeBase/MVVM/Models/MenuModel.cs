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
        public event Action InvokedMainMenuShow;
        public event Action InvokedMainMenuHide;

        public bool IsMenuShowing { get; private set; }
        public bool IsHeroSelectionShowing { get; private set; }
        public bool IsUpgradeSelectionShowing { get; private set; }

        public void EnableHeroSelector()
        {
            InvokedMainMenuHide?.Invoke();
            OpenedHeroSelection?.Invoke();
        }

        public void DisableHeroSelector()
        {
            InvokedMainMenuShow?.Invoke();
            ClosedHeroSelection?.Invoke();
        }

        public void EnableUpgradeSelection()
        {
            InvokedMainMenuHide?.Invoke();
            OpenedUpgradeSelection?.Invoke();
        }

        public void DisableUpgradeSelection()
        {
            InvokedMainMenuShow?.Invoke();
            ClosedUpgradeSelection?.Invoke();
        }

        public void InvokeLeaderBoardShow() => LeaderBoardShowInvoked?.Invoke();
        public void InvokeLeaderBoardHide() => LeaderBoardHideInvoked?.Invoke();
        public void InvokeMainMenuShow() => InvokedMainMenuShow?.Invoke();
        public void InvokeMainMenuHide() => InvokedMainMenuHide?.Invoke();
    }
}