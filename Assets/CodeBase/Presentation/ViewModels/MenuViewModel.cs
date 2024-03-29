using System;
using CodeBase.Domain.Models;

namespace CodeBase.Presentation.ViewModels
{
    public class MenuViewModel
    {
        private readonly MenuModel _menuModel;

        public MenuViewModel(MenuModel menuModel)
        {
            _menuModel = menuModel;
            _menuModel.OpenedUpgradeSelection += OnModelOpenedUpgradeSelection;
            _menuModel.ClosedUpgradeSelection += OnModelClosedUpgradeSelection;
            _menuModel.LeaderBoardShowInvoked += OnLeaderBoardShowInvoked;
            _menuModel.LeaderBoardHideInvoked += OnLeaderBoardHideInvoked;
            _menuModel.InvokedMainMenuShow += OnInvokedMainMenuShow;
            _menuModel.InvokedMainMenuHide += OnInvokedMainMenuHide;
        }

        ~MenuViewModel()
        {
            _menuModel.OpenedUpgradeSelection -= OnModelOpenedUpgradeSelection;
            _menuModel.ClosedUpgradeSelection -= OnModelClosedUpgradeSelection;
            _menuModel.LeaderBoardShowInvoked -= OnLeaderBoardShowInvoked;
            _menuModel.LeaderBoardHideInvoked -= OnLeaderBoardHideInvoked;
            _menuModel.InvokedMainMenuShow -= OnInvokedMainMenuShow;
            _menuModel.InvokedMainMenuHide -= OnInvokedMainMenuHide;
        }

        public event Action OpenedUpgradeSelection;
        public event Action DisabledUpgradeSelection;
        public event Action ShowLeaderBordInvoked;
        public event Action HideLeaderBordInvoked;
        public event Action InvokedMainMenuShow;
        public event Action InvokedMainMenuHide;


        public void EnableHeroSelection() => _menuModel.EnableHeroSelector();
        public void EnableUpgradeSelection() => _menuModel.EnableUpgradeSelection();
        public void DisableUpgradeSelection() => _menuModel.DisableUpgradeSelection();
        public void InvokeLeaderBoardShow() => _menuModel.InvokeLeaderBoardShow();
        public void InvokeLeaderBoardHide() => _menuModel.InvokeLeaderBoardHide();
        public void InvokeSettingsShow() => _menuModel.InvokeSettingsShow();

        private void OnModelOpenedUpgradeSelection() => OpenedUpgradeSelection?.Invoke();
        private void OnModelClosedUpgradeSelection() => DisabledUpgradeSelection?.Invoke();
        private void OnLeaderBoardShowInvoked() => ShowLeaderBordInvoked?.Invoke();
        private void OnLeaderBoardHideInvoked() => HideLeaderBordInvoked?.Invoke();
        private void OnInvokedMainMenuShow() => InvokedMainMenuShow?.Invoke();
        private void OnInvokedMainMenuHide() => InvokedMainMenuHide?.Invoke();
    }
}