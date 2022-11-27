using System;
using CodeBase.Configs;
using CodeBase.MVVM.Models;

namespace CodeBase.MVVM.ViewModels
{
    public class UserNameViewModel
    {
        private readonly UserModel _userModel;
        private readonly MenuModel _menuModel;

        public event Action<string> UserNameChanged;
        public event Action ShowInvoked;
        public event Action HideInvoked;

        public bool IsDefaultName => _userModel.Name == GameConstants.BaseUserName;
        public string Name => _userModel.Name;

        public UserNameViewModel(UserModel userModel, MenuModel menuModel)
        {
            _userModel = userModel;
            _menuModel = menuModel;
            _userModel.NameChanged += OnNameChanged;
            _menuModel.UserNameShowInvoked += () => ShowInvoked?.Invoke();
            _menuModel.UserNameHideInvoked += () => HideInvoked?.Invoke();
        }

        private void OnNameChanged(string name)
        {
            UserNameChanged?.Invoke(name);
            _menuModel.InvokeUserNameHide();
        }

        public void SetUserName(string name) => _userModel.SetName(name);
    }
}