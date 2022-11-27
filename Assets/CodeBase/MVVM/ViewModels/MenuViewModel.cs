using System;
using CodeBase.MVVM.Models;

namespace CodeBase.MVVM.ViewModels
{
    public class MenuViewModel
    {
        private readonly MenuModel _menuModel;

        public MenuViewModel(MenuModel menuModel)
        {
            _menuModel = menuModel;
            _menuModel.OpenedUpgradeSelection += OnModelOpenedUpgradeSelection;
            _menuModel.ClosedUpgradeSelection += OnModelClosedUpgradeSelection;
        }

        public event Action OpenedUpgradeSelection;
        public event Action DisabledUpgradeSelection;

        private void OnModelOpenedUpgradeSelection() => OpenedUpgradeSelection?.Invoke();
        private void OnModelClosedUpgradeSelection() => DisabledUpgradeSelection?.Invoke();

        public void EnableHeroSelection() => _menuModel.EnableHeroSelector();

        public void EnableUpgradeSelection() => _menuModel.EnableUpgradeSelection();

        public void DisableUpgradeSelection() => _menuModel.DisableUpgradeSelection();

        public void InvokeUserNameShow() => _menuModel.InvokeUserNameShow();
    }
}